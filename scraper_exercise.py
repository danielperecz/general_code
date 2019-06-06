import requests
import json
from bs4 import BeautifulSoup

url = "http://dairy.ahdb.org.uk/market-information/#.XPZQe4hKi72"
result = requests.get(url)

data = {}

# If web page loads
if result.status_code == 200:
    source = result.content
    soup = BeautifulSoup(source, "lxml")

    # Table of the 8 rows
    table = soup.find_all("tr")
    for tag in table[1:]:
        # A row with "Key statistics", "Value", "Percentag Changes", "Next update".
        tag = tag.find_all("td")

        # Appending to dictionary
        data[tag[0].text] = {
                                "Value" : tag[1].text,
                                "Percentage Changes" : tag[2].text,
                                "Next update (Week Commencing)" : tag[3].text
                             }

# Create new JSON file
with open("data_file.json", "w") as write_file:
    json.dump(data, write_file)
