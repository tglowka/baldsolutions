import unittest
import json

from unittest.mock import patch
from jsonschema.exceptions import ValidationError
from src.main import SCHEMA_PATH
from src.configs.reader import ConfigReader
from src.configs.configs import NftablesConfig, RedisConnectionConfig, RedisSubscriberConfig
from tests.configs.test_reader import VALID_PATH


class Test_Configs(unittest.TestCase):

    @classmethod
    def setUpClass(self):
        with open(VALID_PATH) as file:
            self.__config = json.load(file)

    @patch('src.configs.reader.ConfigReader')
    def test_NftablesConfig(self, mock_config_reader):
        # Arrange
        expected_nft_startup_commands_path = "./src/configs/setup/nft_startup_commands.json"
        expected_nft_address_rules_path = "./src/configs/setup/nft_address_rules.json"
        expected_port = 1050
        expected_tcp_ports = {80, 22}
        expected_watched_addresses = [
            {
                "ip": "192.168.23.130",
                "tcp_ignore": [80]
            }
        ]

        mock_config_reader.get_config.return_value = self.__config

        config = NftablesConfig(mock_config_reader)

        # Act
        actual_nft_startup_commands_path = config.get_nft_startup_commands_path()
        actual_nft_address_rules_path = config.get_nft_address_rules_path()
        actual_port = config.get_max_port_numer()
        actual_tcp_ports = config.get_tcp_ports()
        actual_watched_addresses = config.get_watched_addresses()

        # Assert
        self.assertEqual(actual_nft_startup_commands_path,
                         expected_nft_startup_commands_path)
        self.assertEqual(actual_nft_address_rules_path,
                         expected_nft_address_rules_path)
        self.assertEqual(actual_port, expected_port)
        self.assertEqual(expected_tcp_ports, actual_tcp_ports)
        self.assertEqual(actual_watched_addresses, expected_watched_addresses)

    @patch('src.configs.reader.ConfigReader')
    def test_RedisConnectionConfig(self, mock_config_reader):
        # Arrange
        expected_host = "127.0.0.1"
        expected_port = 6379
        expected_db = 0
        expected_charset = "utf-8"
        expected_decode_responses = True

        mock_config_reader.get_config.return_value = self.__config

        config = RedisConnectionConfig(mock_config_reader)

        # Act
        actual_host = config.get_host()
        actual_port = config.get_port()
        actual_db = config.get_db()
        actual_charset = config.get_charset()
        actual_decode_responses = config.get_decode_responses()

        # Assert
        self.assertEqual(actual_host, expected_host)
        self.assertEqual(actual_port, expected_port)
        self.assertEqual(actual_db, expected_db)
        self.assertEqual(actual_charset, expected_charset)
        self.assertEqual(actual_decode_responses, expected_decode_responses)

    @patch('src.configs.reader.ConfigReader')
    def test_RedisSubscriberConfig(self, mock_config_reader):
        # Arrange
        expected_subscriber_channel_names = ["test_channel_1", "test_channel_2"]

        mock_config_reader.get_config.return_value = self.__config

        config = RedisSubscriberConfig(mock_config_reader)

        # Act
        actual_subscriber_channel_names = config.get_subscriber_channel_names()

        # Assert
        self.assertEqual(actual_subscriber_channel_names, expected_subscriber_channel_names)


if __name__ == '__main__':
    unittest.main()
