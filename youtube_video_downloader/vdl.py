import urllib.request, urllib.parse
from pytube import YouTube
from re import findall
from sys import argv, exit

path = "C:"       # Default download path

try:
	path = argv[1]
except IndexError:
	pass

try:
	query_string = urllib.parse.urlencode({"search_query" : input("Video Name: ")})
	print("{:>{}}{}".format("Path: ", 12, path))
	print("Downloading...")
	html_content = urllib.request.urlopen("http://www.youtube.com/results?" + query_string)
	search_results = findall(r'href=\"\/watch\?v=(.{11})', html_content.read().decode())
	url = ("http://www.youtube.com/watch?v=" + search_results[0])
	YouTube(url).streams.first().download(path)
except IndexError:
	print("\nError: video not found."), exit()
except KeyboardInterrupt:
	print(), exit()
