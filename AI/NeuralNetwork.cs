using System;
using System.IO;
using SystemPlus.Extensions;

namespace SystemPlus.AI
{
    public sealed class NeuralNetwork
    {
        int[] layer;
        Layer[] layers;

        public NeuralNetwork(int[] _layer)
        {
            layer = new int[_layer.Length];
            Array.Copy(_layer, layer, layer.Length);

            layers = new Layer[layer.Length - 1];

            for (int i = 0; i < layers.Length; i++)
                layers[i] = new Layer(layer[i], layer[i + 1]);
        }

        private NeuralNetwork(Layer[] _layers)
        {
            layers = new Layer[_layers.Length];
            Array.Copy(_layers, layers, _layers.Length);
        }

        public float[] FeedForward(float[] _inputs)
        {
            layers[0].FeedForward(_inputs);
            for (int i = 1; i < layers.Length; i++)
                layers[i].FeedForward(layers[i - 1].outputs);

            return layers[layers.Length - 1].outputs;
        }

        public void BackProp(float[] expected)
        {
            for (int i = layers.Length - 1; i >= 0; i--)
                if (i == layers.Length - 1)
                    layers[i].BackPropOutput(expected);
                else
                    layers[i].BackProp(layers[i + 1].gamma, layers[i + 1].weights);

            for (int i = 0; i < layers.Length; i++)
                layers[i].UpdateWeights();
        }

        public static void Save(NeuralNetwork n, string path)
        {
            long length = 4;
            for (int i = 0; i < n.layers.Length; i++)
                length += n.layers[i].SaveLength();

            byte[] bytes = new byte[length];

            WriteInt(bytes, n.layers.Length, 0);

            for (int i = 0; i < n.layers.Length; i++)
            {
                int locOff = 4;
                for (int j = 0; j < i; j++)
                    locOff += n.layers[j].SaveLength();

                (int ni, int no) s = n.layers[i].L();

                WriteInt(bytes, s.ni, locOff);
                WriteInt(bytes, s.no, locOff + 4);
                locOff += 8;

                int no = n.layers[i].L().no;
                int ni = n.layers[i].L().ni;

                for (int x = 0; x < no; x++)
                    for (int y = 0; y < ni; y++)
                    {
                        WriteFloat(bytes, n.layers[i].weights[x, y], locOff);
                        locOff += 4;
                    }
                Console.WriteLine(i + "/" + (n.layers.Length - 1));
            }

            File.WriteAllBytes(path, bytes);
        }

        public static NeuralNetwork Load(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);

            Layer[] layers = new Layer[ReadInt(bytes, 0)];

            int offset = 4;

            for (int i = 0; i < layers.Length; i++)
            {
                int ni = ReadInt(bytes, offset);
                int no = ReadInt(bytes, offset + 4);
                offset += 8;

                layers[i] = Layer.FromSave(ni, no, bytes.Split(offset, ni * no * 4));
                offset += ni * no * 4;
                Console.WriteLine(i + "/" + (layers.Length - 1));
            }

            return new NeuralNetwork(layers);
        }

        private static int ReadInt(byte[] ar, int offset) => BitConverter.ToInt32(ar, offset);
        private static void WriteInt(byte[] ar, int value, int offset)
        {
            byte[] fbs = BitConverter.GetBytes(value);
            ar[offset] = fbs[0];
            ar[offset + 1] = fbs[1];
            ar[offset + 2] = fbs[2];
            ar[offset + 3] = fbs[3];
        }

        private static float ReadFloat(byte[] ar, int offset) => BitConverter.ToSingle(ar, offset);
        private static void WriteFloat(byte[] ar, float value, int offset)
        {
            byte[] fbs = BitConverter.GetBytes(value);
            ar[offset] = fbs[0];
            ar[offset + 1] = fbs[1];
            ar[offset + 2] = fbs[2];
            ar[offset + 3] = fbs[3];
        }
    }

    internal struct Layer
    {
        private readonly int numberOfInputs;
        private readonly int numberOfOutputs;

        public readonly float[] outputs;
        private float[] inputs;
        public readonly float[,] weights;
        private readonly float[,] weightsDelta;
        public readonly float[] gamma;
        private readonly float[] error;

        private readonly Random rng;

        public Layer(int _numberOfInputs, int _numberOfOutputs)
        {
            numberOfInputs = _numberOfInputs;
            numberOfOutputs = _numberOfOutputs;

            outputs = new float[numberOfOutputs];
            inputs = new float[numberOfInputs];
            weights = new float[numberOfOutputs, numberOfInputs];
            weightsDelta = new float[numberOfOutputs, numberOfInputs];
            gamma = new float[numberOfOutputs];
            error = new float[numberOfOutputs];

            rng = new Random(DateTime.UtcNow.Millisecond);
            InitWeights();
        }

        private Layer(int _numberOfInputs, int _numberOfOutputs, float[,] _weights)
        {
            numberOfInputs = _numberOfInputs;
            numberOfOutputs = _numberOfOutputs;

            outputs = new float[numberOfOutputs];
            inputs = new float[numberOfInputs];
            weights = new float[numberOfOutputs, numberOfInputs];
            Array.Copy(_weights, weights, _weights.Length);
            weightsDelta = new float[numberOfOutputs, numberOfInputs];
            gamma = new float[numberOfOutputs];
            error = new float[numberOfOutputs];

            rng = new Random(DateTime.UtcNow.Millisecond);
        }

        internal static Layer FromSave(int _numberOfInputs, int _numberOfOutputs, byte[] _weights)
        {
            float[,] weights = new float[_numberOfOutputs, _numberOfInputs];

            int offset = 0;

            for (int x = 0; x < _numberOfOutputs; x++)
                for (int y = 0; y < _numberOfInputs; y++)
                {
                    weights[x, y] = ReadFloat(_weights, offset);
                    offset += 4;
                }

            return new Layer(_numberOfInputs, _numberOfOutputs, weights);
        }

        void InitWeights()
        {
            for (int i = 0; i < numberOfOutputs; i++)
                for (int j = 0; j < numberOfInputs; j++)
                    weights[i, j] = (float)rng.NextDouble() - 0.5f;
        }

        public float[] FeedForward(float[] _inputs)
        {
            inputs = _inputs;//Array.Copy(_inputs, inputs, _inputs.Length);

            for (int i = 0; i < numberOfOutputs; i++)
            {
                outputs[i] = 0;
                for (int j = 0; j < numberOfInputs; j++)
                    outputs[i] += inputs[j] * weights[i, j];

                outputs[i] = (float)Math.Tanh(outputs[i]);
            }

            return outputs;
        }

        float TanHDer(float val) => 1f - (val * val);

        public void BackPropOutput(float[] expected)
        {
            for (int i = 0; i < numberOfOutputs; i++)
            {
                error[i] = outputs[i] - expected[i];
            }

            for (int i = 0; i < numberOfOutputs; i++)
                gamma[i] = error[i] * TanHDer(outputs[i]);

            for (int i = 0; i < numberOfOutputs; i++)
                for (int j = 0; j < numberOfInputs; j++)
                    weightsDelta[i, j] = gamma[i] * inputs[j];
        }

        public void BackProp(float[] gammaForawrd, float[,] weightsForward)
        {
            for (int i = 0; i < numberOfOutputs; i++)
            {
                gamma[i] = 0;
                for (int j = 0; j < gammaForawrd.Length; j++)
                    gamma[i] = gammaForawrd[j] * weightsForward[j, i];

                gamma[i] *= TanHDer(outputs[i]);
            }

            for (int i = 0; i < numberOfOutputs; i++)
                for (int j = 0; j < numberOfInputs; j++)
                    weightsDelta[i, j] = gamma[i] * inputs[j];
        }

        public void UpdateWeights()
        {
            for (int i = 0; i < numberOfOutputs; i++)
                for (int j = 0; j < numberOfInputs; j++)
                    weights[i, j] -= weightsDelta[i, j] * 0.00333f;
        }

        internal int SaveLength() => numberOfOutputs * numberOfInputs * 4 + 8;

        internal (int ni, int no) L() => (numberOfInputs, numberOfOutputs);


        private static float ReadFloat(byte[] ar, int offset) => BitConverter.ToSingle(ar, offset);
    }
}
