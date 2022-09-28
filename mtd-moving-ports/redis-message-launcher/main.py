import time
import argparse
import secrets
import datetime
import redis

parser = argparse.ArgumentParser()

parser.add_argument("--lower_boundary_seconds", "-l",
                    help="set the range lower boundary in seconds", type=int)
parser.add_argument("--uppper_boundary_seconds", "-u",
                    help="set the range upper boundary in seconds", type=int)

parser.add_argument("--redis_ip", "-r",
                    help="Redis IP", type=str)

args = parser.parse_args()

REDIS_CONNECTION = redis.Redis(host=args.redis_ip,
                               port=6379,
                               db=0,
                               charset="utf-8",
                               decode_responses=True)

REDIS_CHANNEL = "test_channel_1"


def main():
    lower_boundary_seconds = args.lower_boundary_seconds
    uppper_boundary_seconds = args.uppper_boundary_seconds

    if lower_boundary_seconds is None or uppper_boundary_seconds is None:
        message = "Launcher execution. Time: " + \
            datetime.datetime.utcnow().strftime("%d %m %Y %H:%M:%S")

        publish(message)

        return

    sleep_range = uppper_boundary_seconds - lower_boundary_seconds + 1

    while(True):

        sleep_time_seconds = secrets.randbelow(sleep_range)
        sleep_time_seconds += lower_boundary_seconds

        time.sleep(sleep_time_seconds)

        message = "Launcher execution. Time: " + \
            datetime.datetime.utcnow().strftime("%d %m %Y %H:%M:%S")

        publish(message)


def publish(message):
    REDIS_CONNECTION.publish(REDIS_CHANNEL, message)
    print(f"Publisher: {message}", flush=True)


if __name__ == "__main__":
    main()
