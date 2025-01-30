class Colors:
    OKBLUE = '\033[94m'
    OKGREEN = '\033[92m'
    WARNING = '\033[93m'
    FAIL = '\033[91m'
    ENDC = '\033[0m'

    @staticmethod
    def prompt(text):
        return f"{Colors.OKGREEN}{text}{Colors.ENDC}"

    @staticmethod
    def response(text):
        return f"{Colors.OKBLUE}{text}{Colors.ENDC}"

    @staticmethod
    def warning(text):
        return f"{Colors.WARNING}{text}{Colors.ENDC}"

    @staticmethod
    def error(text):
        return f"{Colors.FAIL}{text}{Colors.ENDC}"

