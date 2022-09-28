import unittest
from unittest import mock
from unittest.mock import mock_open, patch
from src.nftables.nftables_rules_generator import NftablesRulesGenerator, NftablesConfigParser
from src.configs.configs import NftablesConfig
from src.configs.reader import ConfigReader


class Test_NftablesConfigParser(unittest.TestCase):

    @patch('src.configs.configs.NftablesConfig')
    def test_get_ports_to_shuffle_by_address(self, mock_config):

        # Arrange
        mock_config.get_tcp_ports.return_value = {22, 80, 443, 10000}
        mock_config.get_watched_addresses.return_value = [
            {
                "ip": "192.168.23.1",
                "tcp_ignore": [80]
            },
            {
                "ip": "192.168.23.2",
                "tcp_ignore": [22]
            },
            {
                "ip": "192.168.23.3",
                "tcp_ignore": [22, 80, 443, 10000]
            }
        ]

        expected = {
            "192.168.23.1": {22, 443, 10000},
            "192.168.23.2": {80, 443, 10000},
            "192.168.23.3": set()
        }

        parse = NftablesConfigParser(mock_config)

        # Act
        actual = parse.get_ports_to_shuffle_by_address()

        # Assert
        self.assertEqual(actual, expected)


if __name__ == '__main__':
    unittest.main()
