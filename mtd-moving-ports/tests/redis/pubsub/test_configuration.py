import unittest
from unittest.mock import patch

from configs.configs import RedisSubscriberConfiguration


class Test_RedisPubSubConfiguration(unittest.TestCase):

    @patch('src.configs.reader.ConfigurationReader')
    def test_check_configuration_objects(self, mock_configuration_reader):
        """
        Checks the redis pubsub configuration json objects.

        """
        # Arrange
        expected_subscriber_channel_names = [
            "test_channel_1",
            "test_channel_2"
        ]

        mock_configuration_reader.get_configuration_json.return_value = {
            "redis_subscriber_configuration": {
                "subscriber_channel_names": expected_subscriber_channel_names
            }
        }

        redis_subscriber_configuration = RedisSubscriberConfiguration(
            configuration_reader=mock_configuration_reader)

        # Act
        actual_subscriber_channel_names = redis_subscriber_configuration.subscriber_channel_names

        # Assert
        self.assertEqual(actual_subscriber_channel_names,
                         expected_subscriber_channel_names)


if __name__ == '__main__':
    unittest.main()
