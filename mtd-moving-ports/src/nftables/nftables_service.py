import json
from typing import List
from src.configs.configs import NftablesConfig
from src.nftables.nftables_rules_generator import NftablesRulesGenerator
from nftables.nftables import Nftables


class NftablesService:

    def __init__(self, nftables_config: NftablesConfig) -> None:
        self.__COMMANDS = "commands"
        self.__RULES = "rules"
        self.__NFTABLES = "nftables"

        self.__rules_generator = NftablesRulesGenerator(nftables_config)
        self.__nftables = Nftables()

        # loaded once, no need to refresh
        self.__nftables_startup_commands = self.__rules_generator.get_nftables_startup_commands()

        # empty first, dymanically loaded
        self.__nftables_json = {self.__NFTABLES: []}

    def apply_rules(self):
        self.__prepare_nftables_json()
        self.__apply_and_validate()

    def __prepare_nftables_json(self) -> None:
        address_rules = self.__rules_generator.generate_nftables_address_rules()

        self.__cleanup_nftables_json()
        self.__add_startup_commands_to_json()
        self.__add_address_rules_to_json(address_rules)
        self.__validate_nftables_json()

    def __apply_and_validate(self) -> None:
        rc, output, error = self.__nftables.json_cmd(self.__nftables_json)
        self.__validate_nftables_output(rc, output, error)

    def __cleanup_nftables_json(self) -> None:
        self.__nftables_json[self.__NFTABLES] = []

    def __add_startup_commands_to_json(self) -> None:
        parsed_commands = json.loads(self.__nftables_startup_commands)
        self.__nftables_json[self.__NFTABLES].extend(
            parsed_commands[self.__COMMANDS])

    def __add_address_rules_to_json(self, address_rules: List[str]) -> None:
        for rules in address_rules:
            parsed_rules = json.loads(rules)
            self.__nftables_json[self.__NFTABLES].extend(
                parsed_rules[self.__RULES])

    def __validate_nftables_json(self) -> None:
        self.__nftables.json_validate(self.__nftables_json)

    def __validate_nftables_output(self,
                                   rc,
                                   output,
                                   error) -> None:
        if rc != 0:
            print(f"Validate nftables output - error: {error}", flush=True)
            raise NftablesRcException()

        if len(output) != 0:
            print(f"Validate nftables output - error: {output}", flush=True)
            raise NftablesOutputException()


class NftablesRcException(Exception):
    pass


class NftablesOutputException(Exception):
    pass
