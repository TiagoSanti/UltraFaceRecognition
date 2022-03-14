import os
#os.environ['TF_CPP_MIN_LOG_LEVEL'] = '3'
from ArcFaceAPI import *
from Person import Person


def process_and_encode(img):
    img = preprocess(img)
    return image_encoding(img)


def get_temp_image(image_path):
    img = load_image_and_process(image_path)
    return img


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
    #for person in people:
    #    person.show()

    encodings = []
    images_dir = sys.argv[2]
    images_path = os.listdir(images_dir)
    if len(images_path) > 0:
        for image_path in images_path:
            img = get_temp_image(image_path)
            os.remove(image_path)
            img_encoding = image_encoding(img)
            encodings.append(img_encoding)

        for encoding in encodings:
            people_distances = {}
            for person in people:
                encodings_distance = []
                for person_encoding in person.encodings:
                    encodings_distance.append(compare_encodings(encoding, person_encoding))
                avg_person_distance = sum(encodings_distance)/len(encodings_distance)
                print(f'{person.name} avg distance: {avg_person_distance}')
                people_distances[f'{person.name}'] = avg_person_distance


main()
