using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication2.Controllers;
using WebApplication2.Models;
using Xunit;

namespace WebApplication2.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Create_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var controller = new BooksController();
            var model = new BookCreationModel { Titles = "Sample Title", Content = "Sample Content" };

            // Act
            var result = controller.Create(model);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Book created successfully!", (result as OkObjectResult)?.Value);
        }

        [Fact]
        public void Create_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var controller = new BooksController();
            var model = new BookCreationModel { Titles = "", Content = "Sample Content" };

            // Act
            var result = controller.Create(model);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Titles cannot be empty.", (result as BadRequestObjectResult)?.Value);
        }

        [Fact]
        public void GetTitles_ReturnsListOfTitles()
        {
            // Arrange
            var controller = new BooksController();

            // Act
            var result = controller.GetTitles();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<string>>(result);
        }

        

        [Fact]
        public void GetTopTenWords_ValidId_ReturnsTopTenWords()
        {
            // Arrange
            var controller = new BooksController();
            var id = 98;

            // Act
            var result = controller.GetTopTenWords(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<KeyValuePair<string, int>>>(result);
        }

        [Fact]
        public void IsMatchingWord_ValidIdAndWord_ReturnsTrue()
        {
            // Arrange
            var controller = new BooksController();
            var id = 2701;
            var word = "whales";

            // Act
            var result = controller.IsMatchingWord(id, word);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void FindSubstringInWords_ValidIdAndSubWord_ReturnsMatchingWords()
        {
            // Arrange
            var controller = new BooksController();
            var id = 98;
            var subWord = "was";

            // Act
            var result = controller.FindSubstringInWords(id, subWord);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<KeyValuePair<string, int>>>(result);
        }

        
    }
}
