﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Solutions.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using PointOfInterestSkill.Dialogs;
using PointOfInterestSkill.Responses.Main;
using PointOfInterestSkill.Responses.Shared;
using PointOfInterestSkill.Tests.Flow.Strings;
using PointOfInterestSkill.Tests.Flow.Utterances;

namespace PointOfInterestSkill.Tests.Flow
{
    [TestClass]
    [TestCategory("UnitTests")]
    public class FindParkingDialogTests : PointOfInterestSkillTestBase
    {
        /// <summary>
        /// Find parking nearby.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ParkingNearbyTest()
        {
            await GetTestFlow()
                .SendConversationUpdate()
                .AssertReply(AssertContains(POIMainResponses.PointOfInterestWelcomeMessage, null))
                .AssertReply(AssertContains(POIMainResponses.FirstPromptMessage, null))
                .Send(BaseTestUtterances.LocationEvent)
                .Send(FindParkingUtterances.FindParkingNearby)
                .AssertReply(AssertContains(POISharedResponses.MultipleLocationsFound, new string[] { CardStrings.Overview }))
                .Send(BaseTestUtterances.OptionOne)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Details }))
                .Send(BaseTestUtterances.ShowDirections)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Route }))
                .Send(BaseTestUtterances.StartNavigation)
                .AssertReply(CheckForEvent())
                .StartTestAsync();
        }

        [TestMethod]
        public async Task ParkingNearbyThenGoBackTest()
        {
            await GetTestFlow()
                .SendConversationUpdate()
                .AssertReply(AssertContains(POIMainResponses.PointOfInterestWelcomeMessage, null))
                .AssertReply(AssertContains(POIMainResponses.FirstPromptMessage, null))
                .Send(BaseTestUtterances.LocationEvent)
                .Send(FindParkingUtterances.FindParkingNearby)
                .AssertReply(AssertContains(POISharedResponses.MultipleLocationsFound, new string[] { CardStrings.Overview }))
                .Send(BaseTestUtterances.OptionOne)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Details }))
                .Send(GeneralTestUtterances.GoBack)
                .AssertReply(AssertContains(POISharedResponses.MultipleLocationsFound, new string[] { CardStrings.Overview }))
                .Send(BaseTestUtterances.OptionOne)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Details }))
                .Send(BaseTestUtterances.ShowDirections)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Route }))
                .Send(BaseTestUtterances.StartNavigation)
                .AssertReply(CheckForEvent())
                .StartTestAsync();
        }

        /// <summary>
        /// Find nearest parking.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ParkingNearestTest()
        {
            await GetTestFlow()
                .SendConversationUpdate()
                .AssertReply(AssertContains(POIMainResponses.PointOfInterestWelcomeMessage, null))
                .AssertReply(AssertContains(POIMainResponses.FirstPromptMessage, null))
                .Send(BaseTestUtterances.LocationEvent)
                .Send(FindParkingUtterances.FindParkingNearest)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Details }))
                .Send(BaseTestUtterances.ShowDirections)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Route }))
                .Send(BaseTestUtterances.StartNavigation)
                .AssertReply(CheckForEvent())
                .StartTestAsync();
        }

        /// <summary>
        /// Find nearest parking and cancel.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ParkingNearestAndCancelTest()
        {
            await GetTestFlow()
                .SendConversationUpdate()
                .AssertReply(AssertContains(POIMainResponses.PointOfInterestWelcomeMessage, null))
                .AssertReply(AssertContains(POIMainResponses.FirstPromptMessage, null))
                .Send(BaseTestUtterances.LocationEvent)
                .Send(FindParkingUtterances.FindParkingNearest)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Details }))
                .Send(GeneralTestUtterances.Cancel)
                .AssertReply(AssertContains(POISharedResponses.CancellingMessage, null))
                .AssertReply(AssertContains(POIMainResponses.FirstPromptMessage, null))
                .StartTestAsync();
        }

        /// <summary>
        /// Find nearest parking and help.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ParkingNearestAndHelpTest()
        {
            await GetTestFlow()
                .SendConversationUpdate()
                .AssertReply(AssertContains(POIMainResponses.PointOfInterestWelcomeMessage, null))
                .AssertReply(AssertContains(POIMainResponses.FirstPromptMessage, null))
                .Send(BaseTestUtterances.LocationEvent)
                .Send(FindParkingUtterances.FindParkingNearest)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Details }))
                .Send(GeneralTestUtterances.Help)
                .AssertReply(AssertContains(POIMainResponses.HelpMessage, null))
                .AssertReply(AssertContains(null, new string[] { CardStrings.Details }))
                .Send(BaseTestUtterances.ShowDirections)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Route }))
                .Send(BaseTestUtterances.StartNavigation)
                .AssertReply(CheckForEvent())
                .StartTestAsync();
        }

        /// <summary>
        /// Reprompt for current location and find nearest parking.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task RepromptForCurrentAndParkingNearestTest()
        {
            await GetTestFlow()
                .SendConversationUpdate()
                .AssertReply(AssertContains(POIMainResponses.PointOfInterestWelcomeMessage, null))
                .AssertReply(AssertContains(POIMainResponses.FirstPromptMessage, null))
                .Send(FindParkingUtterances.FindParkingNearest)
                .AssertReply(AssertContains(POISharedResponses.PromptForCurrentLocation, null))
                .Send(ContextStrings.Ave)
                .AssertReply(AssertContains(POISharedResponses.CurrentLocationMultipleSelection, new string[] { CardStrings.Overview }))
                .Send(BaseTestUtterances.No)
                .AssertReply(AssertContains(POISharedResponses.PromptForCurrentLocation, null))
                .Send(ContextStrings.Ave)
                .AssertReply(AssertContains(POISharedResponses.CurrentLocationMultipleSelection, new string[] { CardStrings.Overview }))
                .Send(BaseTestUtterances.OptionOne)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Details }))
                .Send(BaseTestUtterances.ShowDirections)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Route }))
                .Send(BaseTestUtterances.StartNavigation)
                .AssertReply(CheckForEvent())
                .StartTestAsync();
        }

        /// <summary>
        /// Find parking near address and prompt for current location.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ParkingNearAddressAndPromptForCurrentTest()
        {
            await GetTestFlow()
                .SendConversationUpdate()
                .AssertReply(AssertContains(POIMainResponses.PointOfInterestWelcomeMessage, null))
                .AssertReply(AssertContains(POIMainResponses.FirstPromptMessage, null))
                .Send(FindParkingUtterances.FindParkingNearAddress)
                .AssertReply(AssertContains(POISharedResponses.MultipleLocationsFound, new string[] { CardStrings.Overview }))
                .Send(BaseTestUtterances.OptionOne)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Details }))
                .Send(BaseTestUtterances.ShowDirections)
                .AssertReply(AssertContains(POISharedResponses.PromptForCurrentLocation, null))
                .Send(ContextStrings.Ave)
                .AssertReply(AssertContains(POISharedResponses.CurrentLocationMultipleSelection, new string[] { CardStrings.Overview }))
                .Send(BaseTestUtterances.OptionOne)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Route }))
                .Send(BaseTestUtterances.StartNavigation)
                .AssertReply(CheckForEvent())
                .StartTestAsync();
        }

        [TestMethod]
        public async Task ParkingNearbyActionTest()
        {
            await GetSkillTestFlow()
                .Send(FindParkingUtterances.FindParkingNearbyAction)
                .AssertReply(AssertContains(POISharedResponses.MultipleLocationsFound, new string[] { CardStrings.Overview }))
                .Send(BaseTestUtterances.OptionOne)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Details }))
                .Send(BaseTestUtterances.ShowDirections)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Route }))
                .Send(BaseTestUtterances.StartNavigation)
                .AssertReply(CheckForEvent())
                .AssertReply(CheckForEoC(true))
                .StartTestAsync();
        }

        [TestMethod]
        public async Task ParkingNearbyZipcodeActionTest()
        {
            await GetSkillTestFlow()
                .Send(FindParkingUtterances.FindParkingNearbyZipcodeAction)
                .AssertReply(AssertContains(POISharedResponses.MultipleLocationsFound, new string[] { CardStrings.Overview }))
                .Send(BaseTestUtterances.OptionOne)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Details }))
                .Send(BaseTestUtterances.ShowDirections)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Route }))
                .Send(BaseTestUtterances.StartNavigation)
                .AssertReply(CheckForEvent())
                .AssertReply(CheckForEoC(true))
                .StartTestAsync();
        }

        [TestMethod]
        public async Task ParkingNearestAndCancelActionTest()
        {
            await GetSkillTestFlow()
                .Send(FindParkingUtterances.FindParkingNearestAction)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Details }))
                .Send(GeneralTestUtterances.Cancel)
                .AssertReply(AssertContains(POISharedResponses.CancellingMessage, null))
                .AssertReply(CheckForEoC(false))
                .StartTestAsync();
        }

        [TestMethod]
        public async Task ParkingNearestAndHelpActionTest()
        {
            await GetSkillTestFlow()
                .Send(FindParkingUtterances.FindParkingNearestAction)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Details }))
                .Send(GeneralTestUtterances.Help)
                .AssertReply(AssertContains(POIMainResponses.HelpMessage, null))
                .AssertReply(AssertContains(null, new string[] { CardStrings.Details }))
                .Send(BaseTestUtterances.ShowDirections)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Route }))
                .Send(BaseTestUtterances.StartNavigation)
                .AssertReply(CheckForEvent())
                .AssertReply(CheckForEoC(true))
                .StartTestAsync();
        }

        [TestMethod]
        public async Task RepromptForCurrentAndParkingNearestActionTest()
        {
            await GetSkillTestFlow()
                .Send(FindParkingUtterances.FindParkingNearestNoCurrentAction)
                .AssertReply(AssertContains(POISharedResponses.PromptForCurrentLocation, null))
                .Send(ContextStrings.Ave)
                .AssertReply(AssertContains(POISharedResponses.CurrentLocationMultipleSelection, new string[] { CardStrings.Overview }))
                .Send(BaseTestUtterances.No)
                .AssertReply(AssertContains(POISharedResponses.PromptForCurrentLocation, null))
                .Send(ContextStrings.Ave)
                .AssertReply(AssertContains(POISharedResponses.CurrentLocationMultipleSelection, new string[] { CardStrings.Overview }))
                .Send(BaseTestUtterances.OptionOne)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Details }))
                .Send(BaseTestUtterances.ShowDirections)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Route }))
                .Send(BaseTestUtterances.StartNavigation)
                .AssertReply(CheckForEvent())
                .AssertReply(CheckForEoC(true))
                .StartTestAsync();
        }

        [TestMethod]
        public async Task ParkingNearestNoRouteActionTest()
        {
            await GetSkillTestFlow()
                .Send(FindParkingUtterances.FindParkingNearestNoRouteAction)
                .AssertReply(AssertContains(null, new string[] { CardStrings.Details }))
                .AssertReply(CheckForEvent())
                .AssertReply(CheckForEoC(true))
                .StartTestAsync();
        }
    }
}
