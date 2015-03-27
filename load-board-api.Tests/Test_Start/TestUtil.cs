using load_board_api.App_Start;
using load_board_api.Dtos;
using load_board_api.Models;
using load_board_api.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace load_board_api.Tests.Test_Start
{
    static class TestUtil
    {
        public static void Compare(
            LocationDto[] expected,
            LocationDto[] actual,
            bool idEqual = true,
            bool nameEqual = true,
            bool deletedEqual = true,
            bool lastUpdatedEqual = true
        )
        {
            int numExpected = expected.Length;
            int numActual = actual.Length;
            Assert.AreEqual(numExpected, numActual);
            for (int i = 0; i < numExpected; i++)
            {
                Compare(
                    expected[i],
                    actual[i],
                    idEqual: idEqual,
                    nameEqual: nameEqual,
                    deletedEqual: deletedEqual,
                    lastUpdatedEqual: lastUpdatedEqual
                );
            }
        }

        public static void Compare(
            LocationDto expected,
            LocationDto actual,
            bool idEqual = true,
            bool nameEqual = true,
            bool deletedEqual = true,
            bool lastUpdatedEqual = true
        ) {
            if (idEqual)
            {
                Assert.AreEqual(expected.Id, actual.Id);
            }
            else
            {
                Assert.AreNotEqual(expected.Id, actual.Id);
            }
            if (nameEqual)
            {
                Assert.AreEqual(expected.Name, expected.Name);
            }
            else
            {
                Assert.AreNotEqual(expected.Name, expected.Name);
            }
            if (deletedEqual)
            {
                Assert.AreEqual(expected.Deleted, actual.Deleted);
            }
            else
            {
                Assert.AreNotEqual(expected.Deleted, actual.Deleted);
            }
            if (lastUpdatedEqual)
            {
                Assert.AreEqual(expected.LastUpdated, actual.LastUpdated);
            }
            else
            {
                Assert.AreNotEqual(expected.LastUpdated, actual.LastUpdated);
            }
        }

        public static void Compare(
            TrailerDto[] expected,
            TrailerDto[] actual,
            bool idEqual = true,
            bool lastUpdatedEqual = true,
            bool deletedEqual = true,
            bool locationIdEqual = true,
            bool locationNameEqual = true,
            bool locationDeletedEqual = true,
            bool locationLastUpdatedEqual = true
        )
        {
            int numExpected = expected.Length;
            int numActual = actual.Length;
            Assert.AreEqual(numExpected, numActual);
            for (int i = 0; i < numExpected; i++)
            {
                Compare(
                    expected[i],
                    actual[i],
                    idEqual:idEqual,
                    lastUpdatedEqual:lastUpdatedEqual,
                    deletedEqual:deletedEqual,
                    locationIdEqual:locationIdEqual,
                    locationNameEqual:locationNameEqual,
                    locationDeletedEqual:locationDeletedEqual,
                    locationLastUpdatedEqual: locationLastUpdatedEqual
                );
            }
        }

        public static void Compare(
            TrailerDto expected,
            TrailerDto actual,
            bool idEqual = true,
            bool lastUpdatedEqual = true,
            bool deletedEqual = true,
            bool locationIdEqual = true,
            bool locationNameEqual = true,
            bool locationDeletedEqual = true,
            bool locationLastUpdatedEqual = true
        ) {
            Compare(
                expected.Location,
                actual.Location,
                idEqual: locationIdEqual,
                nameEqual: locationNameEqual,
                deletedEqual: locationDeletedEqual,
                lastUpdatedEqual: locationLastUpdatedEqual
            );
            if (idEqual)
            {
                Assert.AreEqual(expected.Id, actual.Id);
            }
            else
            {
                Assert.AreNotEqual(expected.Id, actual.Id);
            }
            if (lastUpdatedEqual)
            {
                Assert.AreEqual(expected.LastUpdated, actual.LastUpdated);
            }
            else
            {
                Assert.AreNotEqual(expected.LastUpdated, actual.LastUpdated);
            }
            if (deletedEqual)
            {
                Assert.AreEqual(expected.Deleted, actual.Deleted);
            }
            else
            {
                Assert.AreNotEqual(expected.Deleted, actual.Deleted);
            }
        }
    }
}
