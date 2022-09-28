import redis
from src.configs.configs import RedisConnectionConfig, RedisSubscriberConfig


class RedisSubscriber:

    def __init__(self,
                 subscriber_config: RedisSubscriberConfig,
                 connection_config: RedisConnectionConfig):
        self.__channels = subscriber_config.get_subscriber_channel_names()
        self.__redis_connection = redis.Redis(host=connection_config.get_host(),
                                              port=connection_config.get_port(),
                                              db=connection_config.get_db(),
                                              charset=connection_config.get_charset(),
                                              decode_responses=connection_config.get_decode_responses())

    def subscribe(self, on_message_receive: object) -> None:
        subscriber = self.__redis_connection.pubsub()
        subscriber.subscribe(self.__channels)

        try:
            for message in subscriber.listen():
                print(f"Subscriber: {message}", flush=True)
                if(message["type"] == "message"):
                    on_message_receive()

        except KeyboardInterrupt:
            subscriber.unsubscribe()
