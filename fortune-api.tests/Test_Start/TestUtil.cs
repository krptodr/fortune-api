using fortune_api.App_Start;
using fortune_api.Dtos;
using fortune_api.Models;
using fortune_api.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace fortune_api.Tests.Test_Start
{
    static class TestUtil
    {
        public static void Compare(
            LocationDto[] expected,
            LocationDto[] actual,
            bool idEqual = true,
            bool deletedEqual = true
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
                    deletedEqual: deletedEqual
                );
            }
        }

        public static void Compare(
            LocationDto expected,
            LocationDto actual,
            bool idEqual = true,
            bool deletedEqual = true
        ) {
            if (idEqual)
            {
                Assert.AreEqual(expected.Id, actual.Id);
            }
            else
            {
                Assert.AreNotEqual(expected.Id, actual.Id);
            }
            Assert.AreEqual(expected.Name, expected.Name);
            if (deletedEqual)
            {
                Assert.AreEqual(expected.Deleted, actual.Deleted);
            }
            else
            {
                Assert.AreNotEqual(expected.Deleted, actual.Deleted);
            }
        }

        public static void Compare(
            TrailerDto[] expected,
            TrailerDto[] actual,
            bool idEqual = true,
            bool deletedEqual = true,
            bool locationIdEqual = true,
            bool locationDeletedEqual = true
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
                    deletedEqual:deletedEqual,
                    locationIdEqual:locationIdEqual,
                    locationDeletedEqual:locationDeletedEqual
                );
            }
        }

        public static void Compare(
            TrailerDto expected,
            TrailerDto actual,
            bool idEqual = true,
            bool deletedEqual = true,
            bool locationIdEqual = true,
            bool locationDeletedEqual = true
        ) {
            Compare(
                expected.Location,
                actual.Location,
                idEqual: locationIdEqual,
                deletedEqual: locationDeletedEqual
            );
            if (idEqual)
            {
                Assert.AreEqual(expected.Id, actual.Id);
            }
            else
            {
                Assert.AreNotEqual(expected.Id, actual.Id);
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

        public static void Compare(
            LoadDto expected,
            LoadDto actual,
            bool idEqual = true,
            bool deletedEqual = true,
            bool trailerIdEqual = true,
            bool trailerDeleted = true,
            bool trailerLocationIdEqual = true,
            bool trailerLocationDeletedEqual = true,
            bool originIdEqual = true,
            bool originDeletedEqual = true,
            bool destinationIdEqual = true,
            bool destinationDeletedEqual = true
        )
        {
            Compare(
                expected.Trailer,
                actual.Trailer,
                idEqual: trailerIdEqual,
                deletedEqual: trailerDeleted,
                locationIdEqual: trailerLocationIdEqual,
                locationDeletedEqual: trailerLocationIdEqual
            );
            Compare(
                expected.Origin,
                actual.Origin,
                idEqual: originIdEqual,
                deletedEqual: originDeletedEqual
            );
            Compare(
                expected.Destination,
                actual.Destination,
                idEqual: destinationIdEqual,
                deletedEqual: destinationDeletedEqual
            );
            Assert.AreEqual(expected.CfNum, actual.CfNum);
            Assert.AreEqual(expected.PuNum, actual.PuNum);
            Assert.AreEqual(expected.Status, actual.Status);
            Assert.AreEqual(expected.Type, actual.Type);
            if (deletedEqual)
            {
                Assert.AreEqual(expected.Deleted, actual.Deleted);
            }
            else
            {
                Assert.AreNotEqual(expected.Deleted, actual.Deleted);
            }
            if (idEqual)
            {
                Assert.AreEqual(expected.Id, actual.Id);
            }
            else
            {
                Assert.AreNotEqual(expected.Id, actual.Id);
            }
        }
    }
}
