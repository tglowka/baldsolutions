from typing import Dict
import json
import jsonschema


class ConfigReader:

    def __init__(self, schema_path, config_path) -> None:
        self.__schema_path = schema_path
        self.__config_path = config_path
        self.__config = {}
        self.__read_and_validate_config()

    def get_config(self) -> Dict:
        return self.__config

    def __read_and_validate_config(self) -> None:
        schema = {}
        config = {}

        # read json schema
        with open(self.__schema_path) as file:
            schema = json.load(file)

        # read json
        with open(self.__config_path) as file:
            config = json.load(file)

        # validate json using schema
        jsonschema.validate(instance=config,
                            schema=schema)

        self.__config = config
