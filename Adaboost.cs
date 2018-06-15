using System;
using System.Diagnostics;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.ML;
using Emgu.CV.ML.Structure;
using NUnit.Framework;

//thetraning

namespace AdaBoostTest
{
    [TestFixture]
    public class AdaBoostLetterTest
    {

        private static void ReadLetterData(out Matrix<float> data, out Matrix<float> response)
        {
            string[] rows = System.IO.File.ReadAllLines("letter-recognition_data.data");

            int varCount = rows[0].Split(',').Length - 1;
            data = new Matrix<float>(rows.Length, varCount);
            response = new Matrix<float>(rows.Length, 1);
            int count = 0;
            foreach (string row in rows)
            {
                string[] values = row.Split(',');
                Char c = Convert.ToChar(values[0]);
                response[count, 0] = Convert.ToInt32(c);
                for (int i = 1; i < values.Length; i++)
                    data[count, i - 1] = Convert.ToByte(values[i]);
                count++;
            }
        }

        [Test]
        public void LetterTest()
        {
            Matrix<float> data, responses;
            ReadLetterData(out data, out responses);

            Debug.WriteLine("Data is loaded");

            const int class_count = 26;
            int nsamples_all = data.Rows;
            int ntrain_samples = (int)(nsamples_all * 0.5);
            int var_count = data.Cols;

            // Create boost classifier 

            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //
            // As currently boosted tree classifier in MLL can only be trained
            // for 2-class problems, we transform the training database by
            // "unrolling" each training sample as many times as the number of
            // classes (26) that we have.
            //
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            // 1. unroll the database type mask
            Debug.WriteLine("Unrolling the database...");

            Matrix<float> new_data = new Matrix<float>(ntrain_samples * class_count, var_count + 1);
            Matrix<float> new_responses = new Matrix<float>(ntrain_samples * class_count, 1);

            for (int i = 0; i < ntrain_samples; i++)
            {
                for (int j = 0; j < class_count; j++)
                {
                    int new_row = i * class_count + j;

                    for (int k = 0; k < var_count; k++)
                    {
                        new_data[new_row, k] = data[i, k];
                    }

                    new_data[new_row, var_count] = (float)j;


                    new_responses[new_row, 0] = (float)Convert.ToInt16(responses[i, 0] == j + 'A');
                }
            }

            // 2. create type mask

            Matrix<byte> var_type = new Matrix<byte>(var_count + 2, 1);

            for (int i = 0; i < var_count; i++)
            {
                var_type[i, 0] = (byte)Emgu.CV.ML.MlEnum.VAR_TYPE.NUMERICAL;
            }

            //// the last indicator variable, as well
            //// as the new (binary) response are categorical
            var_type[var_count, 0] = (byte)Emgu.CV.ML.MlEnum.VAR_TYPE.CATEGORICAL;
            var_type[var_count + 1, 0] = (byte)Emgu.CV.ML.MlEnum.VAR_TYPE.CATEGORICAL;

            // 3. train classifier
            Debug.WriteLine("Training the classifier (may take a few minutes)...");

            MCvBoostParams boostParams = new MCvBoostParams();
            boostParams.boostType = Emgu.CV.ML.MlEnum.BOOST_TYPE.REAL;
            boostParams.weakCount = 100;
            boostParams.weightTrimRate = 0.95; // Note : to turn off set to 0
            boostParams.maxDepth = 5;
            boostParams.useSurrogates = false;

            boostParams.maxCategories = 2;

            Boost boost = new Boost();
            boost.Train(new_data, Emgu.CV.ML.MlEnum.DATA_LAYOUT_TYPE.ROW_SAMPLE, new_responses, null, null, var_type,
                        null, boostParams, true);

            Debug.WriteLine("Training complete");

            Matrix<float> temp_sample = new Matrix<float>(1, var_count + 1);
            Matrix<float> weak_responses = new Matrix<float>(1, boostParams.weakCount);

            double train_hr = 0;
            double test_hr = 0;

            // compute prediction error on train and test data
            for (int i = 0; i < nsamples_all; i++)
            {
                int best_class = 0;
                double max_sum = -double.MaxValue;
                double r;


                for (int k = 0; k < var_count; k++)
                    temp_sample[0, k] = data[i, k];

                for (int j = 0; j < class_count; j++)
                {
                    temp_sample[0, var_count] = (float)j;
                    boost.Predict(temp_sample, null, weak_responses, MCvSlice.WholeSeq, false);
                    double sum = CvInvoke.cvSum(weak_responses).v0;
                    if (max_sum < sum)
                    {
                        max_sum = sum;
                        best_class = j + 'A';
                    }
                }

                r = Math.Abs(best_class - responses[i, 0]) < float.Epsilon ? 1 : 0;

                if (i < ntrain_samples)
                    train_hr += r;
                else
                    test_hr += r;
            }

            test_hr /= (double)(nsamples_all - ntrain_samples);
            train_hr /= (double)ntrain_samples;

            Debug.WriteLine("Recognition rate: train = " + train_hr * 100 + ", test = " + test_hr * 100);

            Assert.AreEqual(82.06, train_hr * 100);
            Assert.AreEqual(78.43, test_hr * 100);

            Debug.WriteLine("Number of trees: " + boostParams.weakCount);

            Assert.AreEqual(100, boostParams.weakCount);

        }
    }
}