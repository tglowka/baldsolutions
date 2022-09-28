import unittest
from unittest.mock import patch

from configs.configs import RedisClientConfiguration


class Test_RedisClientConfiguration(unittest.TestCase):

    @patch('src.configs.reader.ConfigurationReader')
    def test_check_configuration_objects(self, mock_configuration_reader):
        """
        Checks the redis client configuration json objects.

        """
        # Arrange
        expected_host = "127.0.0.1"
        expected_port = 6379
        expected_db = 5
        expected_charset = "utf-8"
        expected_decode_responses = True

        mock_configuration_reader.get_configuration_json.return_value = {
            "redis_client_configuration": {
                "host": expected_host,
                "port": expected_port,
                "db": expected_db,
                "charset": expected_charset,
                "decode_responses": expected_decode_responses
            }
        }

        redis_client_configuration = RedisClientConfiguration(
            configuration_reader=mock_configuration_reader)

        # Act

        actual_host = redis_client_configuration.host
        actual_port = redis_client_configuration.port
        actual_db = redis_client_configuration.db
        actual_charset = redis_client_configuration.charset
        actual_decode_responses = redis_client_configuration.decode_responses

        # Assert
        self.assertEqual(actual_host, expected_host)
        self.assertEqual(actual_port, expected_port)
        self.assertEqual(actual_db, expected_db)
        self.assertEqual(actual_charset, expected_charset)
        self.assertEqual(actual_decode_responses, expected_decode_responses)


if __name__ == '__main__':
    unittest.main()
