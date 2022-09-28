import unittest
import json

from unittest.mock import patch
from jsonschema.exceptions import ValidationError
from src.main import SCHEMA_PATH
from src.configs.reader import ConfigReader

VALID_PATH = "./tests/configs/files/valid_configuration.json"


class Test_ConfigReader(unittest.TestCase):

    @classmethod
    def setUpClass(self):
        self.__valid_path = VALID_PATH
        self.__invalid_path = "./tests/configs/files/invalid_configuration.json"

    def test_read_and_validate_configuration_file_invalidConfigurationFileThrowError(self):
        # Act
        try:
            ConfigReader(SCHEMA_PATH, self.__invalid_path)
        except ValidationError:
            pass
        else:
            self.fail('ValidationError not raised')

    def test_get_configuration_json_readAndValidateConfigFileReturnNotEmptyJson(self):

        # Arrange
        expected = {}
        with open(self.__valid_path) as file:
            expected = json.load(file)

        # Act
        config = ConfigReader(SCHEMA_PATH, self.__valid_path).get_config()

        # Assert
        self.assertNotEqual(config, {})


if __name__ == '__main__':
    unittest.main()
