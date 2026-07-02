import uvicorn
from fastapi import FastAPI
from schema import Hits, ServiceInput
from service_processor import ServiceProcessor

app = FastAPI()


@app.get("/")
def main():
    return {"message": "Hello, Zoezi!"}


@app.post("/service/")
def search_service_endpoint(service_input: ServiceInput) -> Hits:
    sp = ServiceProcessor(data_file="data.json")
    return sp.search_service(service_input)


if __name__ == "__main__":
    uvicorn.run("main:app", host="0.0.0.0", port=8123, reload=True)
