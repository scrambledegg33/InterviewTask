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
            _mockRepository.Setup(repo => repo.GetAllFinesAsync())
                .ReturnsAsync(finesEntities);

            // Act
            var result = await _service.GetFinesAsync();

            // Assert
            var finesList = result.ToList();
            Assert.NotNull(finesList);
            Assert.Equal(3, finesList.Count);
        }

        [Fact]
        public async Task GetFinesAsync_WhenCalled_CallsRepositoryOnce()
        {
            // Arrange
            var finesEntities = GetSampleFinesEntities();
            _mockRepository.Setup(repo => repo.GetAllFinesAsync())
                .ReturnsAsync(finesEntities);

            // Act
            await _service.GetFinesAsync();

            // Assert
            _mockRepository.Verify(repo => repo.GetAllFinesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetFinesAsync_WhenCalled_MapsEntitiesToResponses()
        {
            // Arrange
            var finesEntities = new List<FinesEntity>
            {
                new FinesEntity
                {
                    Id = 1,
                    FineNo = "FN-001",
                    FineDate = new DateTime(2024, 1, 15),
                    FineType = FineType.Speeding,
                    CustomerName = "John Doe",
                    VehicleRegNo = "ABC123",
                    VehicleDriverName = "John Doe"
                }
            };
            _mockRepository.Setup(repo => repo.GetAllFinesAsync())
                .ReturnsAsync(finesEntities);

            // Act
            var result = await _service.GetFinesAsync();

            // Assert
            var fine = result.First();
            Assert.Equal(1, fine.Id);
            Assert.Equal("FN-001", fine.FineNo);
            Assert.Equal(new DateTime(2024, 1, 15), fine.FineDate);
            Assert.Equal(FineType.Speeding, fine.FineType);
            Assert.Equal("John Doe", fine.CustomerName);
            Assert.Equal("ABC123", fine.VehicleRegNo);
            Assert.Equal("John Doe", fine.VehicleDriverName);
        }

        [Fact]
        public async Task GetFinesAsync_WhenNoFines_ReturnsEmptyCollection()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAllFinesAsync())
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
                new FinesEntity { Id = 1, FineNo = "FN-001", FineDate = DateTime.Now, FineType = FineType.Speeding, CustomerName = "Test1", VehicleRegNo = "REG1", VehicleDriverName = "Driver1" },
                new FinesEntity { Id = 2, FineNo = "FN-002", FineDate = DateTime.Now, FineType = FineType.Parking, CustomerName = "Test2", VehicleRegNo = "REG2", VehicleDriverName = "Driver2" },
                new FinesEntity { Id = 3, FineNo = "FN-003", FineDate = DateTime.Now, FineType = FineType.RedLightViolation, CustomerName = "Test3", VehicleRegNo = "REG3", VehicleDriverName = "Driver3" },
                new FinesEntity { Id = 4, FineNo = "FN-004", FineDate = DateTime.Now, FineType = FineType.NoInsurance, CustomerName = "Test4", VehicleRegNo = "REG4", VehicleDriverName = "Driver4" },
                new FinesEntity { Id = 5, FineNo = "FN-005", FineDate = DateTime.Now, FineType = FineType.SeatBeltViolation, CustomerName = "Test5", VehicleRegNo = "REG5", VehicleDriverName = "Driver5" }
            };
            _mockRepository.Setup(repo => repo.GetAllFinesAsync())
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
            _mockRepository.Setup(repo => repo.GetAllFinesAsync())
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
                    CustomerName = "John Doe",
                    VehicleRegNo = "ABC123",
                    VehicleDriverName = "John Doe"
                },
                new FinesEntity
                {
                    Id = 2,
                    FineNo = "FN-002",
                    FineDate = new DateTime(2024, 1, 20),
                    FineType = FineType.Parking,
                    CustomerName = "Jane Smith",
                    VehicleRegNo = "XYZ789",
                    VehicleDriverName = "Jane Smith"
                },
                new FinesEntity
                {
                    Id = 3,
                    FineNo = "FN-003",
                    FineDate = new DateTime(2024, 2, 5),
                    FineType = FineType.RedLightViolation,
                    CustomerName = "Bob Johnson",
                    VehicleRegNo = "DEF456",
                    VehicleDriverName = "Bob Johnson"
                }
            };
        }
    }
}