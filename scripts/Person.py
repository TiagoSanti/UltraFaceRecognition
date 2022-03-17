class Person:
    def __init__(self, name):
        self.name = name
        self.encodings = []

    def add_encoding(self, encoding):
        self.encodings.append(encoding)

    def show(self):
        print(f'name: {self.name}, encodings count: {len(self.encodings)}')
