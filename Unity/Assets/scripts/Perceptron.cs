using System;

public class Perceptron
{
    private float[][][] weights = new float[2][][];

    public Perceptron()
    {
        Random random = new Random();
        weights[0] = new float[10][];
        weights[1] = new float[10][];

        for (int i = 0; i < weights[0].Length; i++)
        {
            weights[0][i] = new float[10];
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
        weights[0] = new float[10][];
        weights[1] = new float[10][];

        int newWeightsPosition = 0;
        for (int i = 0; i < weights[0].Length; i++)
        {
            weights[0][i] = new float[10];
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



    public float[] Guess(bool[] inputs)
    {
        float[] normalizedInputs = new float[inputs.Length];
        for (int i = 0; i < inputs.Length; i++)
            normalizedInputs[i] = inputs[i] ? 1f : -1f;
        return new float[] { OutputValue(2, 0, normalizedInputs), OutputValue(2, 1, normalizedInputs) };
    }

    private float OutputValue(int layer, int position, float[] inputs)
    {
        if (layer == 0)
            return inputs[position];

        float finalValue = 0f;
        for (int i = 0; i < weights[layer - 1].Length; i++)
            finalValue += OutputValue(layer - 1, i, inputs) * weights[layer - 1][i][position];

        return ActivationFunction(finalValue);
    }

    private float ActivationFunction(double value)
    {
        return 1.0f / (1.0f + (float)Math.Exp(-value));
    }



    public float[] GetPerceptronDNA()
    {
        int numWeights = weights[0].Length * weights[0][0].Length + weights[1].Length * weights[1][0].Length;
        float[] DNA = new float[numWeights];

        int DNAPosition = 0;

        for (int i = 0; i < weights[0].Length; i++)
            for (int j = 0; j < weights[0][i].Length; j++)
            {
                DNA[DNAPosition] = weights[0][i][j];
                DNAPosition++;
            }
        for (int i = 0; i < weights[1].Length; i++)
            for (int j = 0; j < weights[1][i].Length; j++)
            {
                DNA[DNAPosition] = weights[1][i][j];
                DNAPosition++;
            }

        return DNA;
    }
}