from src.nftables.nftables_service import NftablesService
from src.configs.reader import ConfigReader
from src.configs.configs import NftablesConfig, RedisConnectionConfig, RedisSubscriberConfig
from src.redis.subscriber import RedisSubscriber

SCHEMA_PATH = "./src/configs/setup/configuration_schema.json"
CONFIG_PATH = "./src/configs/setup/configuration.json"


def main():
    config_reader = ConfigReader(SCHEMA_PATH, CONFIG_PATH)
    nftables_service = get_nftables_service(config_reader)
    redis_subscriber = get_redis_subscriber(config_reader)

    redis_subscriber.subscribe(nftables_service.apply_rules)


def get_nftables_service(config_reader: ConfigReader) -> NftablesService:
    config = NftablesConfig(config_reader)
    return NftablesService(config)


def get_redis_subscriber(config_reader: ConfigReader) -> RedisSubscriber:
    connection_config = RedisConnectionConfig(config_reader)
    subscriber_config = RedisSubscriberConfig(config_reader)
    return RedisSubscriber(subscriber_config, connection_config)


if __name__ == "__main__":
    main()
