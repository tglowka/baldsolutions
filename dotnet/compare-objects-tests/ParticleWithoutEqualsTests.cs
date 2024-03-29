﻿using Xunit;

namespace compare_objects_tests
{
    public class ParticleWithoutEqualsTests
    {
        [Fact]
        public void Equals_StructsHaveEqualValueFieldAndTheSameReferenceObject_ReturnsTrue()
        {
            //Arrange
            var internalData = new InternalData();
            var x = 7;

            var particle1 = new ParticleWithoutEquals(x, internalData);
            var particle2 = new ParticleWithoutEquals(x, internalData);

            //Act
            var result = particle1.Equals(particle2);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_StructsHaveNotEqualValueFieldAndTheSameReferenceObject_ReturnsFalse()
        {
            //Arrange
            var internalData = new InternalData();
            var x1 = 7;
            var x2 = 8;

            var particle1 = new ParticleWithoutEquals(x1, internalData);
            var particle2 = new ParticleWithoutEquals(x2, internalData);

            //Act
            var result = particle1.Equals(particle2);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_StructsHaveEqualValueFieldAndDifferentReferenceObject_ReturnsFalse()
        {
            //Arrange
            var internalData1 = new InternalData();
            var internalData2 = new InternalData();
            var x = 7;

            var particle1 = new ParticleWithoutEquals(x, internalData1);
            var particle2 = new ParticleWithoutEquals(x, internalData2);

            //Act
            var result = particle1.Equals(particle2);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_StructsHaveNotEqualValueFieldAndDifferentReferenceObject_ReturnsFalse()
        {
            //Arrange
            var internalData1 = new InternalData();
            var internalData2 = new InternalData();
            var x1 = 7;
            var x2 = 8;

            var particle1 = new ParticleWithoutEquals(x1, internalData1);
            var particle2 = new ParticleWithoutEquals(x2, internalData2);

            //Act
            var result = particle1.Equals(particle2);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_StructAndNull_ReturnsFalse()
        {
            //Arrange
            var internalData = new InternalData();
            var x = 7;

            var particle = new ParticleWithoutEquals(x, internalData);

            //Act
            var result = particle.Equals(null);

            //Assert
            Assert.False(result);
        }
    }
}
