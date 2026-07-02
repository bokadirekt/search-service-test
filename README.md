# Project Title

Mohammed's submission of the search service.

## Description

A technical submission for my solution of the search service.

## Getting Started

This project uses uv

### Dependencies

* Dependencies are included in pyproject.toml file, and locked in uv.lock

### Running the project

* To run the project, use `uv run main.py` which uses port 8123 on localhost
* To access the endpoint, please go to `localhost:8123/docs` where you can find the "service" endpoint
To run the tests, please use `uv run python -m unittest tests/test_service_processor.py`

### Executing program

* Sample input for the endpoint is:

```
{
  "serviceName": "massage",
  "userLocation": {
  "lat": 59.3156428,
  "lng": 18.0561182999999
  }
}
```

## Authors

Mohammed Bakheet

## Acknowledgments

This project is developed for zoezi