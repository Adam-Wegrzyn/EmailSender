using NUnit.Framework;
using EmailSender.Exceptions;

namespace EmailSender.Tests
{
    public class CsvValidatorUnitTest
    {
        [Test]
        public void Validate_Invalid_EmptyFile_ShouldThrow()
        {
            //arrange
            using var file = File.OpenRead("Files/mergeEmail_invalid_EmptyFile.csv");
            var expectedMessage = "File cannot be empty!";

            //act + assert
            var ex = Assert.Throws<EmptyFileException>(() => CsvValidator.Validate(file));
            Assert.True(ex.Message.Contains(expectedMessage));
        }

        [Test]
        public void Validate_Invalid_EmptyRecipient_ShouldThrow()
        {
            //arrange
            using var file = File.OpenRead("Files/mergeEmail_invalid_EmptyRecipient.csv");
            var expectedMessage = "At least one recipient must be provided!";

            //act + assert
            var ex = Assert.Throws<RecipientNotFoundException>(() => CsvValidator.Validate(file));
            Assert.True(ex.Message.Contains(expectedMessage));
        }

        [Test]
        public void Validate_Valid_ShouldPass()
        {
            //arrange
            using var file = File.OpenRead("Files/mergeEmail_Valid.csv");

            //act
            var isValidated = CsvValidator.Validate(file);

            //assert
            Assert.True(isValidated);
        }
    }
}