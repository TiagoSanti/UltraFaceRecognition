import os
from ArcFaceAPI import *
from Person import Person


def process_and_encode(img):
    img = preprocess(img)
    return image_encoding(img)


def load_people_list():
    try:
        database_path = sys.argv[1]  # c# execution
    except IndexError:
        print('No directory argument found')
        database_path = r'C:\Dev\Github\TiagoSanti\UltraFaceRecognition\database'  # only script execution

    people = []
    people_encodings_path = f'{database_path}\\encodings'
    people_encodings_dirs = os.listdir(people_encodings_path)

    for person_encodings_dir in people_encodings_dirs:
        person = Person(person_encodings_dir)

        person_encodings_files_path = f'{people_encodings_path}\\{person_encodings_dir}'
        person_encodings_files = os.listdir(person_encodings_files_path)

        for person_encoding_file in person_encodings_files:
            person_encoding_file_path = f'{person_encodings_files_path}\\{person_encoding_file}'
            encoding = np.load(person_encoding_file_path)
            person.add_encoding(encoding)

        people.append(person)
    return people


def main():
    people = load_people_list()
    for person in people:
        person.show()


main()
