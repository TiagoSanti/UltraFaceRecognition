import os
import math
import time
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
    try:
        images_dir = sys.argv[2]  # csharp execution
    except IndexError:
        print('No directory argument found')
        images_dir = r'C:\Dev\Github\TiagoSanti\UltraFaceRecognition\temp'
    while True:
        images_path = os.listdir(images_dir)
        if len(images_path) > 0:
            for image_path in images_path:
                img = get_temp_image(images_dir+'\\'+image_path)
                encoding = image_encoding(img)
                people_distances = {}
                for person in people:
                    person_encodings_scores = []
                    for person_encoding in person.encodings:
                        distance = compare_encodings(encoding, person_encoding)
                        square_distance = math.sqrt(distance)
                        score = 1/square_distance
                        person_encodings_scores.append(score)
                    person_avg_score = sum(person_encodings_scores)/len(person_encodings_scores)
                    people_distances[f'{person.name}'] = person_avg_score
                    #sys.stdout.flush()
                # TO DO
                # best_score = max(people_distances, people_distances.get)
        print('')


        print('sleeping')
        time.sleep(5)
        print('sleep finished')


main()