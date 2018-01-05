using System;

public class Perceptron
{
    private float[][][] weights = new float[2][][];
    private const int NUM_FIRST_LAYER_NEURONS = 9;

    public Perceptron()
    {
        weights[0] = new float[9][];
        weights[1] = new float[9][];

        Random random = new Random();

        for (int i = 0; i < weights[0].Length; i++)
        {
            weights[0][i] = new float[9];
            for (int j = 0; j < weights[0][i].Length; j++)
                weights[0][i][j] = (float)(random.NextDouble()) * 2.0f - 1.0f;
        }
        for (int i = 0; i < weights[1].Length; i++)
        {
            weights[1][i] = new float[2];
            for (int j = 0; j < weights[1][i].Length; j++)
                weights[1][i][j] = (float)(random.NextDouble()) * 2.0f - 1.0f;
        }
    }

    public Perceptron(float[] newWeights)
    {
        weights[0] = new float[9][];
        weights[1] = new float[9][];

        Random random = new Random();

        int newWeightsPosition = 0;
        for (int i = 0; i < weights[0].Length; i++)
        {
            weights[0][i] = new float[9];
            for (int j = 0; j < weights[0][i].Length; j++)
            {
                weights[0][i][j] = newWeights[newWeightsPosition];
                newWeightsPosition++;
            }
        }
        for (int i = 0; i < weights[1].Length; i++)
        {
            weights[1][i] = new float[2];
            for (int j = 0; j < weights[1][i].Length; j++)
            {
                weights[1][i][j] = newWeights[newWeightsPosition];
                newWeightsPosition++;
            }
        }
    }

    public float[] guess(float[] inputs)
    {
        return new float[0];
    }

    private float outputValue(int layer, int position, float[] inputs)
    {
        if (layer == 0) // is an input
            return inputs[position];
        return sigmoid();
    }


    private float sigmoid(double value)
    {
        return 1.0f / (1.0f + (float)Math.Exp(-value));
    }

    public float[] getPerceptronADN() {
        int numWeights = weights[0].Length * weights[0][0].Length + weights[1].Length * weights[1][0].Length;
        float[] ADN = new float[numWeights];

        int ADNPosition = 0;

        for (int i = 0; i < weights[0].Length; i++)
            for (int j = 0; j < weights[0][i].Length; j++)
            {
                ADN[ADNPosition] = weights[0][i][j];
                ADNPosition++;
            }
        for (int i = 0; i < weights[1].Length; i++)
            for (int j = 0; j < weights[1][i].Length; j++)
            {
                ADN[ADNPosition] = weights[1][i][j];
                ADNPosition++;
            }

        return ADN;
    }
}