import json


with open('result.raw', 'r') as myfile:
    raw = myfile.read().replace("\n", ",")

with open('classify.sh', 'r') as myfile:
    img = myfile.read().split("\n")

imglist = []

for l in img:
    spl = l.split()
    i = spl.index("--image_file") + 1
    imagename = spl[i].split("/")[-1]
    imglist.append(imagename)

print(len(imglist))

r = []
for x in raw.split(";"):
    if len(x) > 5:
        r.append(x)
r = r[2:]

resultimgobj = {}
resultobjimg = {}

for i, l in enumerate(r):
    ll = l.split(")")
    resultimgobj[imglist[i]] = []
    for item in ll:
        if len(item) > 2:
            item = item.strip()
            if item.startswith(","):
                item = item[1:]
            if item.startswith("\\"):
                item = item[3:]
            obj, score = item.split("(")
            score = score.replace("score = ", "")
            if float(score) > 0.1:
                resultimgobj[imglist[i]].append({"score": score, "obj": obj})
                if obj in resultobjimg:
                    resultobjimg[obj].append({"score": score, "img": imglist[i]})
                else:
                    resultobjimg[obj] = [{"score": score, "img": imglist[i]}]


with open('resultimgobj.json', 'w') as fp:
    json.dump(resultimgobj, fp)

with open('resultobjimg.json', 'w') as fp:
    json.dump(resultobjimg, fp)

print(resultobjimg.keys())