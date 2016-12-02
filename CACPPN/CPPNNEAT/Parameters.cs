﻿using System;
using CPPNNEAT.CA;
using CPPNNEAT.CPPN;
using CPPNNEAT.Utils;

namespace CPPNNEAT
{

	class Parameters
	{
		public EAParameters EA;
		public CPPNParameters CPPN;
		public CAParameters CA;

		public Parameters()
		{
			EA = new EAParameters();
			CPPN = new CPPNParameters();
			CA = new CAParameters();
		}
	}
	struct EAParameters
	{
		public const int PopulationSize = 200;
		public const int MaximumRuns = 12;
		public const int SpeciesImprovementTriesBeforeDeath = 15;
		public static INeatCA neatCA = new BaseNeatCA(); //needs subclasses that "are the experiment" like FloweringCA <- is a growth setup CA

		public const float ExcessSimilarityWeight = 1.0f;
		public const float DisjointSimilarityWeight = 1.0f;
		public const float WeightDifferenceSimilarityWeight = 0.4f;
		public const int SetNToOneLimit = 12;

		public const bool IsSeededRun = true;
		public const int RandomSeed = 42;

		public const double SameFloatWithinReason = 0.01f; //should be scaled by how variant the fitness is
	}

	struct CAParameters
	{
		public int NeighbourHoodSize;
		public int CellStateCount;
		public int CellWorldWidth;
		public int CellWorldHeight;
	}

	struct CPPNParameters
	{
		public const float InitialMaxConnectionWeight = 0.13f;
		public const float WeightMin = -2.0f, WeightMax = 2.0f;

		public static int CPPNetworkInputSize;
		public static int CPPNetworkOutputSize;

		private static TupleList<ActivationFunctionType,float> FunctionChances = new TupleList<ActivationFunctionType, float>
				{	// make sure it all sums to 1.0 (100%)
					{ ActivationFunctionType.Sinusodial,    0.2f },
					{ ActivationFunctionType.Gaussian,   0.14f },
					{ ActivationFunctionType.AbsoluteValue,     0.13f },
					{ ActivationFunctionType.PyramidAbsoluteValue, 0.17f},
					{ActivationFunctionType.Modulo, 0.2f },
					{ ActivationFunctionType.Linear,    0.16f }
				};

		public static TupleList<float, ActivationFunctionType> ActivationFunctionChanceIntervals
		{
			get
			{
				var intervals = new TupleList<float, ActivationFunctionType>();
				float compoundedValue = 0.0f;
				foreach(Tuple<ActivationFunctionType, float> tuple in FunctionChances)
				{
					compoundedValue += tuple.Item2;
					intervals.Add(compoundedValue, tuple.Item1);
				}
				return intervals;
			}
		}
	}
}