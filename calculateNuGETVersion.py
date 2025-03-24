import argparse
from json import load

parser = argparse.ArgumentParser()
parser.add_argument("stream", type=str)
parser.add_argument("changelist", type=str)

args = parser.parse_args()

stream = args.stream
changelist = args.changelist

with open("nuget_version.json", "rt") as fIn:

    nuget_version = load(fIn)

    stream = stream.lower()
    stream = stream.replace("_", "")
    stream = stream.replace("-", "")
    stream = stream.replace(" ", "")

    if not "type" in nuget_version or nuget_version["type"] == "":
        if stream == "main" or stream == "master":
            nuget_version["type"] = ""
        elif stream == "develop" or stream == "development":
            nuget_version["type"] = "beta"
        else:
            nuget_version["type"] = "alpha." + stream
    nuget_version["change"] = changelist

    if stream == "main" or stream == "master":
        version = "%(major)s.%(minor)s.%(patch)s" % nuget_version
    else:
        version = "%(major)s.%(minor)s.%(patch)s.%(type)s.%(change)s" % nuget_version
    print(version, end="")
