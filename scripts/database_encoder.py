import os
from os import path
from ArcFaceAPI import *


def check_if_image_encoding_exists(encodings_files, image_file):
    return f'{image_file}.npy' in encodings_files


def main():
    try:
        database_path = sys.argv[1]
    except IndexError:
        print('No arguments found')
        database_path = r'C:\Users\Tiago Santi\Documents\GitHub\UltraFaceRecognition\database'

    os.chdir(database_path)
    people_encodings_path = f'{database_path}\\encodings'
    people_images_path = f'{database_path}\\images'

    if not os.path.exists(people_encodings_path):
        os.chdir(database_path)
        os.mkdir('encodings')

    people_images_dirs = os.listdir(people_images_path)

    if len(people_images_dirs) > 0:
        for person_images_dir in people_images_dirs:
            person_name = (path.split(person_images_dir))[1]

            person_encodings_path = f'{database_path}\\encodings\\{person_name}'

            if os.path.exists(person_encodings_path):
                person_encodings_files = os.listdir(person_encodings_path)
            else:
                person_encodings_files = []
                os.chdir(people_encodings_path)
                os.mkdir(person_name)
            os.chdir(person_encodings_path)

            person_images_path = f'{people_images_path}\\{person_name}'
            person_images = os.listdir(person_images_path)

            if len(person_images) > 0:
                for person_image in person_images:
                    person_image_path = f'{person_images_path}\\{person_image}'

                    if not check_if_image_encoding_exists(person_encodings_files, person_image):
                        img = load_image_and_process(person_image_path)
                        print(f'encoding image: {person_image}')
                        encoding = image_encoding(img)
                        np.save(person_image, encoding)
                    else:
                        print(f'skipping {person_image}..')


main()
