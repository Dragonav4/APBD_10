using APBD_s31722_11.Controllers;
using APBD_s31722_11.DataLayer.Models;
using APBD_s31722_11.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace Tests;

public class PatientControllerTests
{
    private static PatientController CreatePatientController(out Mock<IPrescriptionService> mockService)
    {
        mockService = new Mock<IPrescriptionService>();
        return new PatientController(mockService.Object);
    }

    [Fact]
    public async Task GetPatientWithPrescriptionById_Returns200_WhenFound()
    {
        var controller = CreatePatientController(out var mockService);
        var dto = new PatientWithPrescriptionsDto();

        mockService
            .Setup(s => s.GetPatientWithPrescriptionsAsync(It.IsAny<int>()))
            .ReturnsAsync(dto);
        var result = await controller.GetPatientWithPrescriptionById(1);
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, ok.StatusCode);
    }
    
    [Fact]
    public async Task GetPatientWithPrescriptionById_Returns400_WhenIdIsInvalid()
    {
        var controller = CreatePatientController(out _);
        var result = await controller.GetPatientWithPrescriptionById(0);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400,badRequestResult.StatusCode);
    }
    
}