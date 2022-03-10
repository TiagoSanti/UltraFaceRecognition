import os
from os import path
import numpy as np
from ArcFaceAPI import *


def check_if_image_encoding_exists(encodings_files, image_file):
    return image_file + ".npy" in encodings_files


database_path = r"..\database"
os.chdir(database_path)
database_path = os.getcwd()
people_encodings_path = database_path + r"\encodings"
people_images_path = database_path + r"\images"

if not os.path.exists(database_path + r"\encodings"):
    os.chdir(database_path)
    os.mkdir('encodings')

people_images_dirs = os.listdir(people_images_path)


if len(people_images_dirs) > 0:
    for person_images_dir in people_images_dirs:
        person_name = (path.split(person_images_dir))[1]

        person_encodings_path = database_path + r"\encodings" + "\\" + person_name

        if os.path.exists(person_encodings_path):
            person_encodings_files = os.listdir(person_encodings_path)

        else:
            os.chdir(people_encodings_path)
            os.mkdir(person_name)
        os.chdir(person_encodings_path)

        person_images_path = people_images_path + "\\" + person_name
        print(path.exists(person_images_path))
        person_images = os.listdir(person_images_path)

        if len(person_images) > 0:
            for person_image in person_images:
                person_image_path = person_images_path + "\\" + person_image

                if not check_if_image_encoding_exists(person_encodings_files, person_image):
                    img = load_image_and_process(person_image_path)
                    encoding = image_encoding(img)
                    print(type(encoding))
                    np.save(person_image, encoding)
