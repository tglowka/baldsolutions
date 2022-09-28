import secrets
from typing import List, Set, Dict
from src.configs.configs import NftablesConfig


class NftablesRulesGenerator:
    def __init__(self, nftables_config: NftablesConfig) -> None:

        self.__NFT_SOURCE_ADDRESS_TOKEN = "{{SOURCE_ADDRESS}}"
        self.__NFT_DESTINATION_PORT_TOKEN = "\"{{DESTINATION_PORT}}\""
        self.__NFT_REDIRECTED_PORT_TOKEN = "\"{{REDIRECTED_PORT}}\""
        self.__NFT_PROTOCOL_TOKEN = "{{PROTOCOL}}"
        self.__protocol = 'tcp'

        self.__nftables_config = nftables_config
        self.__nftables_config_parser = NftablesConfigParser(nftables_config)

    def get_nftables_startup_commands(self) -> str:
        return self.__nftables_config_parser.get_nft_startup_commands_content()

    def generate_nftables_address_rules(self) -> List[str]:
        rules_list = []

        for ip, ports in self.__nftables_config_parser.get_ports_to_shuffle_by_address().items():
            destination_ports = ports
            redirection_ports = self.__choose_closed_ports(len(ports))

            for destination_port, redirection_port in zip(destination_ports, redirection_ports):
                rules_list.append(self.__get_template_with_replaced_tokens(
                    source_address=ip,
                    destination_port=destination_port,
                    redirected_port=redirection_port,
                    protocol=self.__protocol))

        return rules_list

    def __choose_closed_ports(self, count) -> Set[int]:
        closed_ports = set()
        max_port = self.__nftables_config.get_max_port_numer() + 1

        while(len(closed_ports) < count):
            port_number = secrets.randbelow(max_port)

            if (port_number == 0):
                continue

            if (self.__tcp_ports_contains(port_number)):
                continue

            closed_ports.add(port_number)

        return closed_ports

    def __tcp_ports_contains(self, port_number: int) -> bool:
        return port_number in self.__nftables_config.get_tcp_ports()

    def __get_template_with_replaced_tokens(self,
                                            source_address: str,
                                            destination_port: int,
                                            redirected_port: int,
                                            protocol: str) -> str:
        rules = self.__nftables_config_parser.get_nft_address_rules_content() \
            .replace(self.__NFT_SOURCE_ADDRESS_TOKEN, source_address) \
            .replace(self.__NFT_DESTINATION_PORT_TOKEN, str(destination_port)) \
            .replace(self.__NFT_REDIRECTED_PORT_TOKEN, str(redirected_port)) \
            .replace(self.__NFT_PROTOCOL_TOKEN, protocol)

        return rules


class NftablesConfigParser:

    def __init__(self, nftables_config: NftablesConfig) -> None:
        self.__IP = "ip"
        self.__TCP_IGNORE = "tcp_ignore"
        self.__nftables_config = nftables_config

    def get_ports_to_shuffle_by_address(self) -> Dict[str, Set[int]]:
        results = {}

        watched_addresses = self.__get_watched_addresses_by_address()
        tcp_ports = self.__nftables_config.get_tcp_ports()

        for ip, ports_to_ignore in watched_addresses.items():
            results[ip] = tcp_ports.difference(ports_to_ignore)

        return results

    def get_nft_address_rules_content(self) -> str:
        return self.__read_file_as_string(self.__nftables_config.get_nft_address_rules_path())

    def get_nft_startup_commands_content(self) -> str:
        return self.__read_file_as_string(self.__nftables_config.get_nft_startup_commands_path())

    def __get_watched_addresses_by_address(self) -> Dict[str, Set[int]]:
        watched_addresses = {}
        addresses = self.__nftables_config.get_watched_addresses()

        for address in addresses:
            ip = address[self.__IP]
            ports_to_ignore = set(address[self.__TCP_IGNORE])
            watched_addresses[ip] = ports_to_ignore

        return watched_addresses

    def __read_file_as_string(self, file_path) -> str:
        file_content = ""

        with open(file_path) as file:
            file_content = file.read()

        return file_content
