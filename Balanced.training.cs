﻿﻿// This file was auto-generated by ML.NET Model Builder. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using Microsoft.ML;

namespace CollegeScorePredictor
{
    public partial class Balanced
    {
        /// <summary>
        /// Retrains model using the pipeline generated as part of the training process. For more information on how to load data, see aka.ms/loaddata.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <param name="trainData"></param>
        /// <returns></returns>
        public static ITransformer RetrainPipeline(MLContext mlContext, IDataView trainData)
        {
            var pipeline = BuildPipeline(mlContext);
            var model = pipeline.Fit(trainData);

            return model;
        }

        /// <summary>
        /// build the pipeline that is used from model builder. Use this function to retrain model.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Categorical.OneHotEncoding(@"IsConference", @"IsConference", outputKind: OneHotEncodingEstimator.OutputKind.Indicator)      
                                    .Append(mlContext.Transforms.ReplaceMissingValues(new []{new InputOutputColumnPair(@"Week", @"Week"),new InputOutputColumnPair(@"TeamId", @"TeamId"),new InputOutputColumnPair(@"TeamWins", @"TeamWins"),new InputOutputColumnPair(@"TeamConference", @"TeamConference"),new InputOutputColumnPair(@"TeamFirstDowns", @"TeamFirstDowns"),new InputOutputColumnPair(@"TeamTotalYards", @"TeamTotalYards"),new InputOutputColumnPair(@"TeamNetPassingYards", @"TeamNetPassingYards"),new InputOutputColumnPair(@"TeamRushingYards", @"TeamRushingYards"),new InputOutputColumnPair(@"TeamTotalPenalties", @"TeamTotalPenalties"),new InputOutputColumnPair(@"TeamTotalPenaltyYards", @"TeamTotalPenaltyYards"),new InputOutputColumnPair(@"TeamDefensiveTacklesForLoss", @"TeamDefensiveTacklesForLoss"),new InputOutputColumnPair(@"TeamSpecialTeamsPoints", @"TeamSpecialTeamsPoints"),new InputOutputColumnPair(@"OpponentTeamId", @"OpponentTeamId"),new InputOutputColumnPair(@"OpponentWins", @"OpponentWins"),new InputOutputColumnPair(@"OpponentConference", @"OpponentConference"),new InputOutputColumnPair(@"OpponentFirstDownsAllowed", @"OpponentFirstDownsAllowed"),new InputOutputColumnPair(@"OpponentTotalYardsAllowed", @"OpponentTotalYardsAllowed"),new InputOutputColumnPair(@"OpponentPassingYardsAllowed", @"OpponentPassingYardsAllowed"),new InputOutputColumnPair(@"OpponentRushingYardsAllowed", @"OpponentRushingYardsAllowed"),new InputOutputColumnPair(@"OpponentTotalPenalties", @"OpponentTotalPenalties"),new InputOutputColumnPair(@"OpponentTotalPenaltyYards", @"OpponentTotalPenaltyYards"),new InputOutputColumnPair(@"OpponentDefensiveTacklesForLoss", @"OpponentDefensiveTacklesForLoss"),new InputOutputColumnPair(@"OpponentSpecialTeamsPointsAllowed", @"OpponentSpecialTeamsPointsAllowed"),new InputOutputColumnPair(@"TeamTurnoverMargin", @"TeamTurnoverMargin"),new InputOutputColumnPair(@"OpponentTurnoverMargin", @"OpponentTurnoverMargin")}))      
                                    .Append(mlContext.Transforms.Concatenate(@"Features", new []{@"IsConference",@"Week",@"TeamId",@"TeamWins",@"TeamConference",@"TeamFirstDowns",@"TeamTotalYards",@"TeamNetPassingYards",@"TeamRushingYards",@"TeamTotalPenalties",@"TeamTotalPenaltyYards",@"TeamDefensiveTacklesForLoss",@"TeamSpecialTeamsPoints",@"OpponentTeamId",@"OpponentWins",@"OpponentConference",@"OpponentFirstDownsAllowed",@"OpponentTotalYardsAllowed",@"OpponentPassingYardsAllowed",@"OpponentRushingYardsAllowed",@"OpponentTotalPenalties",@"OpponentTotalPenaltyYards",@"OpponentDefensiveTacklesForLoss",@"OpponentSpecialTeamsPointsAllowed",@"TeamTurnoverMargin",@"OpponentTurnoverMargin"}))      
                                    .Append(mlContext.Transforms.NormalizeMinMax(@"Features", @"Features"))      
                                    .Append(mlContext.Regression.Trainers.FastTreeTweedie(new FastTreeTweedieTrainer.Options(){NumberOfLeaves=4,MinimumExampleCountPerLeaf=70,NumberOfTrees=258,MaximumBinCountPerFeature=480,FeatureFraction=0.644393553287318,LearningRate=0.160569055442132,LabelColumnName=@"TeamScore",FeatureColumnName=@"Features"}));

            return pipeline;
        }
    }
}
