﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace CPPNNEAT.NEAT
{
	class Species : IComparer<Individual>
	{
		public readonly int speciesID;
		public List<Individual> populace { get; private set; }
		public float SpeciesFitness { get; private set; }
		public int AllowedPopulaceCount { get; set; }

		public Species(int speciesID)
		{
			this.speciesID = speciesID;
			SpeciesFitness = 0.0f;
			AllowedPopulaceCount = EAParameters.PopulationSize; //TODO no, not quite
			populace = new List<Individual>();
		}

		public void Initialize(IDCounters IDs)
		{
			for(int i = 0; i < EAParameters.PopulationSize; i++)
			{
				Individual indie = new Individual(IDs);
				indie.Initialize();
			}
		}

		public void EvaluatePopulace()
		{
			Parallel.ForEach(populace, indie => { indie.Evaluate(); });
		}

		public void MakeNextGeneration(IDCounters IDs)
		{
			//this is where the compare should come into play for fitness selection.
			//and "foreach" by the allowed amount
		}

		public int Compare(Individual x, Individual y)
		{
			return x.CompareTo(y);
		}
	}
}