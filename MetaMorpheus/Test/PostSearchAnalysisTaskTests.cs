﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineLayer;
using MassSpectrometry;
using Nett;
using NUnit.Framework;
using Proteomics.ProteolyticDigestion;
using Proteomics.RetentionTimePrediction;
using TaskLayer;

namespace Test
{
    [TestFixture]
    public static class PostSearchAnalysisTaskTests
    {
        [Test]
        public static void AllResultsAndResultsTxtTests()
        {
            //First test that AllResults and Results display correct numbers of peptides and psms with q-value filter on
            string myTomlPath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestData\Task1-SearchTaskconfig.toml");
            SearchTask searchTaskLoaded = Toml.ReadFile<SearchTask>(myTomlPath, MetaMorpheusTask.tomlConfig);
            string outputFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestData\PostSearchAnalysisTaskTest");
            string myFile1 = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestData\sliced_b6.mzML");
            string myFile2 = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestData\sliced_b6_2.mzML");
            string myDatabase = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestData\LowResSnip_B6_mouse_11700_117500pruned.xml");

            EverythingRunnerEngine engineToml = new(new List<(string, MetaMorpheusTask)> { ("postSearchAnalysisTaskTestOutput", searchTaskLoaded) }, new List<string> { myFile1, myFile2 }, new List<DbForTask> { new DbForTask(myDatabase, false) }, outputFolder);
            engineToml.Run();

            string allResultsFile = Path.Combine(outputFolder, "allResults.txt");
            string[] allResults = File.ReadAllLines(allResultsFile);
            Assert.AreEqual("All target PSMs with q-value = 0.01: 41", allResults[12]);
            Assert.AreEqual("All target peptides with q-value = 0.01 : 32", allResults[13]);
            Assert.AreEqual("All target protein groups with q-value = 0.01 (1% FDR): 29", allResults[14]);
            Assert.AreEqual("sliced_b6 target PSMs with q-value = 0.01: 37", allResults[18]);
            Assert.AreEqual("sliced_b6_2 target PSMs with q-value = 0.01: 4", allResults[22]);
            Assert.AreEqual("Target protein groups within 1 % FDR in sliced_b6: 29", allResults[24]);
            Assert.AreEqual("Target protein groups within 1 % FDR in sliced_b6_2: 3", allResults[25]);
            Assert.AreEqual("sliced_b6 Target peptides with q-value = 0.01 : 29", allResults[26]);
            Assert.AreEqual("sliced_b6_2 Target peptides with q-value = 0.01 : 3", allResults[28]);


            string resultsFile = Path.Combine(outputFolder, @"postSearchAnalysisTaskTestOutput\results.txt");
            string[] results = File.ReadAllLines(resultsFile);
            Assert.AreEqual("All target PSMs with q-value = 0.01: 41", results[7]);
            Assert.AreEqual("All target peptides with q-value = 0.01 : 32", results[8]);
            Assert.AreEqual("All target protein groups with q-value = 0.01 (1% FDR): 29", results[9]);
            Assert.AreEqual("sliced_b6 target PSMs with q-value = 0.01: 37", results[13]);
            Assert.AreEqual("sliced_b6_2 target PSMs with q-value = 0.01: 4", results[17]);
            Assert.AreEqual("Target protein groups within 1 % FDR in sliced_b6: 29", results[19]);
            Assert.AreEqual("Target protein groups within 1 % FDR in sliced_b6_2: 3", results[20]);
            Assert.AreEqual("sliced_b6 Target peptides with q-value = 0.01 : 29", results[21]);
            Assert.AreEqual("sliced_b6_2 Target peptides with q-value = 0.01 : 3", results[23]);

            Directory.Delete(outputFolder, true);

            //Second test that AllResults and Results display correct numbers of peptides and psms with PEP q-value filter on
            myTomlPath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestData\Task2-SearchTaskconfig.toml");
            searchTaskLoaded = Toml.ReadFile<SearchTask>(myTomlPath, MetaMorpheusTask.tomlConfig);
            engineToml = new EverythingRunnerEngine(new List<(string, MetaMorpheusTask)> { ("postSearchAnalysisTaskTestOutput", searchTaskLoaded) }, new List<string> { myFile1, myFile2 }, new List<DbForTask> { new DbForTask(myDatabase, false) }, outputFolder);
            engineToml.Run();

            allResultsFile = Path.Combine(outputFolder, "allResults.txt");
            allResults = File.ReadAllLines(allResultsFile);
            Assert.AreEqual("All target PSMs with pep q-value = 0.01: 46", allResults[12]);
            Assert.AreEqual("All target peptides with pep q-value = 0.01 : 35", allResults[13]);
            Assert.AreEqual("All target protein groups with q-value = 0.01 (1% FDR): 29", allResults[14]);
            Assert.AreEqual("sliced_b6 target PSMs with pep q-value = 0.01: 39", allResults[18]);
            Assert.AreEqual("sliced_b6_2 target PSMs with pep q-value = 0.01: 7", allResults[22]);
            Assert.AreEqual("Target protein groups within 1 % FDR in sliced_b6: 29", allResults[24]);
            Assert.AreEqual("Target protein groups within 1 % FDR in sliced_b6_2: 3", allResults[25]);
            Assert.AreEqual("sliced_b6 Target peptides with pep q-value = 0.01 : 30", allResults[26]);
            Assert.AreEqual("sliced_b6_2 Target peptides with pep q-value = 0.01 : 6", allResults[28]);


            resultsFile = Path.Combine(outputFolder, @"postSearchAnalysisTaskTestOutput\results.txt");
            results = File.ReadAllLines(resultsFile);
            Assert.AreEqual("All target PSMs with pep q-value = 0.01: 46", results[7]);
            Assert.AreEqual("All target peptides with pep q-value = 0.01 : 35", results[8]);
            Assert.AreEqual("All target protein groups with q-value = 0.01 (1% FDR): 29", results[9]);
            Assert.AreEqual("sliced_b6 target PSMs with pep q-value = 0.01: 39", results[13]);
            Assert.AreEqual("sliced_b6_2 target PSMs with pep q-value = 0.01: 7", results[17]);
            Assert.AreEqual("Target protein groups within 1 % FDR in sliced_b6: 29", results[19]);
            Assert.AreEqual("Target protein groups within 1 % FDR in sliced_b6_2: 3", results[20]);
            Assert.AreEqual("sliced_b6 Target peptides with pep q-value = 0.01 : 30", results[21]);
            Assert.AreEqual("sliced_b6_2 Target peptides with pep q-value = 0.01 : 6", results[23]);

            Directory.Delete(outputFolder, true);
        }
    }
}
