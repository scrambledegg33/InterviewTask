using Fines.Core.Dtos;
using Fines.Core.Enums;
using Fines.Data.Models;
using Fines.Services;
using Moq;

namespace Fines.Tests
{
    public class FinesServiceTests
    {
        private readonly Mock<IFinesRepository> _mockRepository;
        private readonly FinesService _service;

        public FinesServiceTests()
        {
            _mockRepository = new Mock<IFinesRepository>();
            _service = new FinesService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetFinesAsync_WhenCalled_ReturnsAllFines()
        {
            // Arrange
            var finesEntities = GetSampleFinesEntities();
            _mockRepository.Setup(repo => repo.GetFinesAsync(null))
                .ReturnsAsync(finesEntities);

            // Act
            var result = await _service.GetFinesAsync();
            var finesList = result.ToList();

            // Assert
            Assert.NotNull(finesList);
            Assert.Equal(5, finesList.Count);
        }

        [Theory]
        [InlineData(FineType.Speeding)]
        [InlineData(FineType.RedLightViolation)]
        public async Task GetFinesFilteredByFineTypeAsync_WhenCalled_ReturnsAllFinesOfAGivenFineType(FineType fineType)
        {
            // Arrange
            var finesEntities = GetSampleFinesEntities();
            _mockRepository.Setup(repo => repo.GetFinesAsync(It.Is<FinesFilter>(f => f != null && f.FineType == fineType)))
                .ReturnsAsync((FinesFilter? filter) => finesEntities.Where(f => f.FineType == filter!.FineType).ToList());
            var filter = new FinesFilter { FineType = fineType };
            // Act
            var result = await _service.GetFinesAsync(filter);
            var finesList = result.ToList();

            // Assert
            Assert.NotNull(finesList);
            Assert.All(finesList, f => Assert.Equal(fineType, f.FineType));
            var expectedCount = finesEntities.Count(f => f.FineType == fineType);
            Assert.Equal(expectedCount, finesList.Count);
        }

        //[Theory]
        //[InlineData("RERG334")]
        //public async Task GetFinesAsync_WhenVehicleRegIsNotValid_Returns404NotFound(string vehicleReg)
        //{
        //    // Arrange
        //    var finesEntities = GetSampleFinesEntities();
        //    _mockRepository.Setup(repo => repo.GetFinesAsync(It.Is<FinesFilter>(f => f != null && f.VehicleReg == vehicleReg)))
        //        .ReturnsAsync([]);
        //    var filter = new FinesFilter { FineType = fineType };

        //    // Act
        //    var result = await _service.GetFinesFilteredByFineTypeAsync(fineType);
        //    var finesList = result.ToList();

        //    // Assert
        //    Assert.NotNull(finesList);
        //    Assert.All(finesList, f => Assert.Equal(fineType, f.FineType));
        //    var expectedCount = finesEntities.Count(f => f.FineType == fineType);
        //    Assert.Equal(expectedCount, finesList.Count);
        //}

        [Fact]
        public async Task GetFinesAsync_WhenCalled_CallsRepositoryOnce()
        {
            // Arrange
            var finesEntities = GetSampleFinesEntities();
            _mockRepository.Setup(repo => repo.GetFinesAsync(null))
                .ReturnsAsync(finesEntities);

            // Act
            await _service.GetFinesAsync();

            // Assert
            _mockRepository.Verify(repo => repo.GetFinesAsync(null), Times.Once);
        }

        [Fact]
        public async Task GetFinesAsync_WhenCalled_MapsEntitiesToResponses()
        {
            // Arrange
            var vehicle = new VehicleEntity
            {
                Id = 1,
                RegistrationNumber = "ABC123",
                Make = "Ford",
                Model = "Focus",
                Color = "Blue",
                Year = 2020
            };

            var finesEntities = new List<FinesEntity>
            {
                new FinesEntity
                {
                    Id = 1,
                    FineNo = "FN-001",
                    FineDate = new DateTime(2024, 1, 15),
                    FineType = FineType.Speeding,
                    VehicleId = 1,
                    Vehicle = vehicle,
                    VehicleDriverName = "John Doe"
                }
            };
            _mockRepository.Setup(repo => repo.GetFinesAsync(null))
                .ReturnsAsync(finesEntities);

            // Act
            var result = await _service.GetFinesAsync();

            // Assert
            var fine = result.First();
            Assert.Equal(1, fine.Id);
            Assert.Equal("FN-001", fine.FineNo);
            Assert.Equal(new DateTime(2024, 1, 15), fine.FineDate);
            Assert.Equal(FineType.Speeding, fine.FineType);
            Assert.Equal("ABC123", fine.VehicleRegNo);
            Assert.Equal("John Doe", fine.VehicleDriverName);
        }

        [Fact]
        public async Task GetFinesAsync_WhenNoFines_ReturnsEmptyCollection()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetFinesAsync(null))
                .ReturnsAsync(new List<FinesEntity>());

            // Act
            var result = await _service.GetFinesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetFinesAsync_MapsAllFineTypes_Correctly()
        {
            // Arrange
            var finesEntities = new List<FinesEntity>
            {
                new FinesEntity { Id = 1, FineNo = "FN-001", FineDate = DateTime.Now, FineType = FineType.Speeding, VehicleId = 1, Vehicle = new VehicleEntity { Id = 1, RegistrationNumber = "REG1" }, VehicleDriverName = "Driver1" },
                new FinesEntity { Id = 2, FineNo = "FN-002", FineDate = DateTime.Now, FineType = FineType.Parking, VehicleId = 2, Vehicle = new VehicleEntity { Id = 2, RegistrationNumber = "REG2" }, VehicleDriverName = "Driver2" },
                new FinesEntity { Id = 3, FineNo = "FN-003", FineDate = DateTime.Now, FineType = FineType.RedLightViolation, VehicleId = 3, Vehicle = new VehicleEntity { Id = 3, RegistrationNumber = "REG3" }, VehicleDriverName = "Driver3" },
                new FinesEntity { Id = 4, FineNo = "FN-004", FineDate = DateTime.Now, FineType = FineType.NoInsurance, VehicleId = 4, Vehicle = new VehicleEntity { Id = 4, RegistrationNumber = "REG4" }, VehicleDriverName = "Driver4" },
                new FinesEntity { Id = 5, FineNo = "FN-005", FineDate = DateTime.Now, FineType = FineType.SeatBeltViolation, VehicleId = 5, Vehicle = new VehicleEntity { Id = 5, RegistrationNumber = "REG5" }, VehicleDriverName = "Driver5" }
            };
            _mockRepository.Setup(repo => repo.GetFinesAsync(null))
                .ReturnsAsync(finesEntities);

            // Act
            var result = await _service.GetFinesAsync();

            // Assert
            var finesList = result.ToList();
            Assert.Equal(FineType.Speeding, finesList[0].FineType);
            Assert.Equal(FineType.Parking, finesList[1].FineType);
            Assert.Equal(FineType.RedLightViolation, finesList[2].FineType);
            Assert.Equal(FineType.NoInsurance, finesList[3].FineType);
            Assert.Equal(FineType.SeatBeltViolation, finesList[4].FineType);
        }

        [Fact]
        public async Task GetFinesAsync_WhenRepositoryThrowsException_PropagatesException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetFinesAsync(null))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.GetFinesAsync());
        }

        private static List<FinesEntity> GetSampleFinesEntities()
        {
            return new List<FinesEntity>
            {
                new FinesEntity
                {
                    Id = 1,
                    FineNo = "FN-001",
                    FineDate = new DateTime(2024, 1, 15),
                    FineType = FineType.Speeding,
                    VehicleId = 1,
                    Vehicle = new VehicleEntity { Id = 1, RegistrationNumber = "ABC123", Make = "Ford", Model = "Focus", Color = "Blue", Year = 2020 },
                    VehicleDriverName = "John Doe"
                },
                new FinesEntity
                {
                    Id = 2,
                    FineNo = "FN-002",
                    FineDate = new DateTime(2024, 1, 20),
                    FineType = FineType.Parking,
                    VehicleId = 2,
                    Vehicle = new VehicleEntity { Id = 2, RegistrationNumber = "XYZ789", Make = "Volkswagen", Model = "Golf", Color = "Silver", Year = 2021 },
                    VehicleDriverName = "Jane Smith"
                },
                new FinesEntity
                {
                    Id = 3,
                    FineNo = "FN-003",
                    FineDate = new DateTime(2024, 2, 5),
                    FineType = FineType.RedLightViolation,
                    VehicleId = 3,
                    Vehicle = new VehicleEntity { Id = 3, RegistrationNumber = "DEF456", Make = "BMW", Model = "3 Series", Color = "Black", Year = 2022 },
                    VehicleDriverName = "Bob Johnson"
                },
                new FinesEntity
                {
                    Id = 4,
                    FineNo = "FN-004",
                    FineDate = new DateTime(2024, 1, 23),
                    FineType = FineType.SeatBeltViolation,
                    VehicleId = 4,
                    Vehicle = new VehicleEntity { Id = 4, RegistrationNumber = "RRR111", Make = "Honda", Model = "Jazz", Color = "White", Year = 2012 },
                    VehicleDriverName = "Ryan Jones"
                },
                new FinesEntity
                {
                    Id = 5,
                    FineNo = "FN-005",
                    FineDate = new DateTime(2024, 2, 10),
                    FineType = FineType.Speeding,
                    VehicleId = 5,
                    Vehicle = new VehicleEntity { Id = 5, RegistrationNumber = "WER789", Make = "Dacia", Model = "Sandero", Color = "Gold", Year = 2023 },
                    VehicleDriverName = "Jeff Baker"
                }
            };
        }
    }
}