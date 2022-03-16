import os
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
        print('No db directory argument found')
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


def clear():
    os.system('cls')


def main():
    people = load_people_list()
    try:
        images_dir = sys.argv[2]  # csharp execution
    except IndexError:
        print('No temp directory argument found')
        images_dir = r'C:\Dev\Github\TiagoSanti\UltraFaceRecognition\temp'
    while True:
    # for i in range(1):
        start = time.time()
        images_path = np.asarray(os.listdir(images_dir))
        if images_path.size > 0:
            for image_path in images_path:
                img = get_temp_image(images_dir+'\\'+image_path)
                encoding = image_encoding(img)
                people_distances = {}
                for person in people:
                    person_encodings_scores = []  # TODO convert to np.array
                    person_encodings = np.asarray(person.encodings)

                    for person_encoding in person_encodings:
                        person_encodings_scores.append(compare_encodings(encoding, person_encoding))

                    person_encodings_scores = np.asarray(person_encodings_scores)
                    # print(f'Distance: {person_encodings_scores}')
                    person_encodings_scores = np.square(person_encodings_scores)
                    # print(f'Square distance: {person_encodings_scores}')
                    person_encodings_scores = person_encodings_scores**-1
                    # print(f'Score (1/SD): {person_encodings_scores}\n')
                    person_avg_score = person_encodings_scores.mean()
                    people_distances[f'{person.name}'] = person_avg_score

                best_person = max(people_distances, key=people_distances.get)
                print(f'------- Best person match for {image_path}: {best_person} -> Score {people_distances.get(best_person)} --------')
                sys.stdout.flush()

        del images_path
        del person_encodings
        del person_encodings_scores
        print(f'Encoding execution time: {time.time() - start} seconds')
        print('')

        time.sleep(5)


main()

