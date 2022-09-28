from typing import Dict, Set, List
from src.configs.reader import ConfigReader


class NftablesConfig:

    def __init__(self, config_reader: ConfigReader) -> None:
        self.__NFTABLES_SERVICE_CONFIGURATION = "nftables_service_configuration"
        self.__TCP_PORTS = "tcp_ports"
        self.__WATCHED_ADDRESSES = "watched_addresses"
        self.__NFT_STARTUP_COMMANDS_PATH = "nft_startup_commands_path"
        self.__NFT_ADDRESS_RULES_PATH = "nft_address_rules_path"
        self.__MAX_PORT_NUMBER = "max_port_number"

        config = config_reader.get_config()
        self.__nftables_config = config[self.__NFTABLES_SERVICE_CONFIGURATION]

    def get_nft_startup_commands_path(self) -> str:
        return self.__nftables_config[self.__NFT_STARTUP_COMMANDS_PATH]

    def get_nft_address_rules_path(self) -> str:
        return self.__nftables_config[self.__NFT_ADDRESS_RULES_PATH]

    def get_max_port_numer(self) -> int:
        return self.__nftables_config[self.__MAX_PORT_NUMBER]

    def get_tcp_ports(self) -> Set[int]:
        return set(self.__nftables_config[self.__TCP_PORTS])

    def get_watched_addresses(self) -> List[Dict]:
        return self.__nftables_config[self.__WATCHED_ADDRESSES]


class RedisConnectionConfig:
    def __init__(self, config_reader: ConfigReader):

        self.__REDIS_CONNECTION_CONFIGURATION = "redis_connection_configuration"
        self.__HOST = "host"
        self.__PORT = "port"
        self.__DB = "db"
        self.__CHARSET = "charset"
        self.__DECODE_RESPONSES = "decode_responses"

        config = config_reader.get_config()
        self.__redis_config = config[self.__REDIS_CONNECTION_CONFIGURATION]

    def get_host(self) -> str:
        return self.__redis_config[self.__HOST]

    def get_port(self) -> int:
        return self.__redis_config[self.__PORT]

    def get_db(self) -> int:
        return self.__redis_config[self.__DB]

    def get_charset(self) -> str:
        return self.__redis_config[self.__CHARSET]

    def get_decode_responses(self) -> bool:
        return self.__redis_config[self.__DECODE_RESPONSES]


class RedisSubscriberConfig:

    def __init__(self, config_reader: ConfigReader):

        self.__REDIS_SUBSCRIBER_CONFIGURATION = "redis_subscriber_configuration"
        self.__SUBSCRIBER_CHANNEL_NAMES = "subscriber_channel_names"

        config = config_reader.get_config()
        self.__redis_config = config[self.__REDIS_SUBSCRIBER_CONFIGURATION]

    def get_subscriber_channel_names(self) -> List[str]:
        return self.__redis_config[self.__SUBSCRIBER_CHANNEL_NAMES]
