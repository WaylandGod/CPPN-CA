﻿using System;
using System.Collections.Generic;
using CPPNNEAT.Utils;

namespace CPPNNEAT.EA
{
	class Mutation
	{

		public static Genome Mutate(Genome genome, IDCounters IDs)
		{
			Genome newGenome = null;
			foreach(MutationType type in Enum.GetValues(typeof(MutationType)))
			{
				if(NEAT.random.DoMutation(type))
					genome = MutateOfType(type, genome, IDs);
			}
			newGenome = genome; //TODO a "copy genome type of thing here
			return newGenome;
		}

		private static Genome MutateOfType(MutationType type, Genome genome, IDCounters IDs)
		{
			switch(type)
			{
			case MutationType.ChangeWeight:
				return ChangeWeight(genome);
			case MutationType.AddConnection:
				return AddConnection(genome, IDs);
			case MutationType.AddNode:
				return AddNode(genome, IDs);
			case MutationType.ChangeFunction: //and then maybe "mutateCurrentFunction type"
				return ChangeNodeFunction(genome, IDs);
			default:
				return genome;
			}
		}

		private static Genome AddNode(Genome genome, IDCounters IDs)
		{
			ConnectionGene connectionToSplitt = NEAT.random.ConnectionGene(genome);

			NodeGene newNode = new NodeGene(IDs.NodeGeneID,
										genome.nodeGenes.Count,
										NodeType.Hidden,
										NEAT.random.ActivationFunctionType());

			ConnectionGene firstHalfGene = new ConnectionGene(IDs.ConnectionGeneID,
															connectionToSplitt.fromNodeID,
															newNode.nodeID,
															true,
															1.0f);

			ConnectionGene secondHalfGene = new ConnectionGene(IDs.ConnectionGeneID,
															newNode.nodeID,
															connectionToSplitt.fromNodeID,
															true,
															NEAT.random.NextFloat()*CPPNetworkParameters.InitialMaxConnectionWeight);

			connectionToSplitt.isEnabled = false;
			genome.connectionGenes.Add(firstHalfGene);
			genome.connectionGenes.Add(secondHalfGene);
			return genome;
		}

		private static Genome AddConnection(Genome genome, IDCounters IDs)
		{
			//NO RECCURENT BULLSHITT.
			//CHECK HIS PAPER for any good explanations for this
			NodeGene fromNode = NEAT.random.NotOutputNodeGene(genome);
			NodeGene toNode = NEAT.random.NotInputNodeGene(genome); // is it so simple I can just make a "get node from After fromNode" ?
			ConnectionGene conGene = new ConnectionGene(IDs.ConnectionGeneID,
														fromNode.nodeID,
														toNode.nodeID,
														true, //was this supposed to be weighted random for new ones?
														NEAT.random.InitialConnectionWeight());
			genome.connectionGenes.Add(conGene);
			return genome;
		}

		private static Genome ChangeWeight(Genome genome)
		{
			ConnectionGene connGene = NEAT.random.ConnectionGene(genome);
			float newWeight = (connGene.connectionWeight + NEAT.random.NextFloat() * 2.0f * MutationChances.MutatWeightAmount
																			- MutationChances.MutatWeightAmount).ClampWeight();
			return genome;
		}

		private static Genome ChangeNodeFunction(Genome genome, IDCounters IDs) //needs to impact the species placement, cus a sinus function contra a gaussian in the same spot makes a hell of a difference!
		{
			//gotta be a new node with the same nodeID but new nodeGeneID
			NEAT.random.NotInputNodeGene(genome);
			//genome.nodeGenes[nodeIndex].nodeInputFunction = new CPPN.ActivationFunction(); //"get random function HERE
			return genome;
		}

		public static Genome Crossover(Individual indie1, Individual indie2)
		{
			Genome genome1 = indie1.genome;
			Genome genome2 = indie2.genome;
			if(indie1.Fitness.SameWithinReason(indie2.Fitness))
				return SameFitnessRandomCrossOver(genome1, genome2);

			bool parent1HasMostNodes = genome1.nodeGenes.Count > genome2.nodeGenes.Count; //not GENE IDs

			bool mostFitIsIndie1 = indie1.Fitness > indie2.Fitness;
			Genome childGenome = new Genome();
			if(parent1HasMostNodes)
			{
				childGenome.nodeGenes.Add(mostFitIsIndie1 ? genome1.nodeGenes[0] : genome2.nodeGenes[0]); //this type of stuff
			} else
			{

			}

			return childGenome;
		}

		private static Genome SameFitnessRandomCrossOver(Genome genome1, Genome genome2)
		{
			Genome childGenome = new Genome(); //QUESTION is; given that they have "the same fitness" WITHIN the species can we assume they are "the same", count wise?
											   //must be heavily redone
			int shortestNodeGeneList = genome1.nodeGenes.Count < genome2.nodeGenes.Count ?
									   genome1.nodeGenes.Count :
									   genome2.nodeGenes.Count;

			for(int i = 0; i < shortestNodeGeneList; i++)
			{
				if(NEAT.random.NextBoolean())
					childGenome.nodeGenes.Add(genome1.nodeGenes[i]);
				else
					childGenome.nodeGenes.Add(genome1.nodeGenes[i]);
			}
			//TODO what for the remaining genes?

			//must be heavily redone
			int shortestConnectionGeneList = genome1.connectionGenes.Count < genome2.connectionGenes.Count ?
									   genome1.connectionGenes.Count :
									   genome2.connectionGenes.Count;

			for(int i = 0; i < shortestNodeGeneList; i++)
			{
				if(NEAT.random.NextBoolean())
					childGenome.connectionGenes.Add(genome1.connectionGenes[i]);
				else
					childGenome.connectionGenes.Add(genome1.connectionGenes[i]);
			}
			//TODO what for the remaining genes?

			return childGenome;
		}
	}

	enum MutationType //by ordering this you can order the mutations in case of several for the same genome
	{// like adding a node and a connection before changing a weight might be nice, so that every weight is present.
		AddNode,
		AddConnection,
		ChangeWeight,
		ChangeFunction
	}
}