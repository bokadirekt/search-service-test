from schema import Position, Results, Hits, ServiceInput
from typing import List
import json
import logging

logger = logging.getLogger(__name__)

class ServiceProcessor:
    def __init__(self, data_file: str = "data.json"):
        self.services = self.load_service_data(data_file)

    def load_service_data(self, data_file: str) -> List[Results]:
        # this loads data from json file
        try:
            with open(data_file, "r") as f:
                all_services = json.load(f)
                logger.info(f"Loaded {len(all_services)} records from {data_file}")
            return all_services
        except FileNotFoundError:
            logger.error(f"Data file {data_file} not found.")
            raise FileNotFoundError(f"Data file {data_file} not found.")
        except json.JSONDecodeError:
            logger.error(f"Data file {data_file} is not a valid JSON file.")
            raise ValueError(f"Data file {data_file} is not a valid JSON file.")

    def calculate_distance(
        self, user_position: Position, service_position: Position
    ) -> float:
        """
        Calculate the distance between two geographical points using the Haversine formula.
        Return the distance in kilometers.
        """
        from math import radians, sin, cos, sqrt, atan2

        R = 6371.0  # Radius of the Earth in kilometers

        user_lat = radians(user_position.lat)
        user_lng = radians(user_position.lng)
        service_lat = radians(service_position.lat)
        service_lng = radians(service_position.lng)

        dlng = service_lng - user_lng
        dlat = service_lat - user_lat

        a = sin(dlat / 2) ** 2 + cos(user_lat) * cos(service_lat) * sin(dlng / 2) ** 2
        c = 2 * atan2(sqrt(a), sqrt(1 - a))

        distance = R * c

        return distance

    def get_geo_distance(self, user_location: Position) -> List[dict]:
        service_distances = []
        for service in self.services:
            service_position = Position(
                lat=service["position"]["lat"], lng=service["position"]["lng"]
            )
            distance = self.calculate_distance(user_location, service_position)
            service["distance"] = distance  # Add distance to the service dictionary
            service_distances.append(service)
        return service_distances

    def calculate_service_score(self, services: list) -> List[dict]:
        """
        Calculate the score based on distance.
        Score is calculated as follows:
        - If the service is within 1 km, score = 5
        - If the service is within 2 km, score = 4
        - If the service is within 3 km, score = 3
        - If the service is within 4 km, score = 2
        - If the service is within 5 km, score = 1
        - If the service is beyond 5 km, score = 0
        We must also add the name matching logic in the scoring system,
        but due to time constraints, we will focus on the distance-based scoring for now.
        """
        services_with_score = []
        for service in services:
            if 0 < service["distance"] < 1:  # If the service is within 1 km
                service["score"] = 5
            elif 1 < service["distance"] < 2:
                service["score"] = 4
            elif 2 < service["distance"] < 3:
                service["score"] = 3
            elif 3 < service["distance"] < 4:
                service["score"] = 2
            elif 4 < service["distance"] < 5:
                service["score"] = 1
            else:
                service["score"] = 0
            if service["distance"] < 1:
                # after calculating the distance, change type to str for output compatiblity
                service["distance"] = f"{service['distance'] * 1000:.0f}m"
            else:
                service["distance"] = f"{service['distance']:.2f}km"
            services_with_score.append(service)

        return services_with_score

    def search_service(self, service_input: ServiceInput) -> Hits:
        # loop through the results and filter by service_name
        user_service_name = service_input.serviceName
        user_location = service_input.userLocation
        service_distances = self.get_geo_distance(user_location)
        # sort the services by distance
        service_distances.sort(key=lambda x: (x["distance"]))
        services_with_score = self.calculate_service_score(service_distances)
        # sort the services by score
        services_with_score.sort(key=lambda x: (x["score"]), reverse=True)
        matched_by_name = [
            service
            for service in services_with_score
            if user_service_name.lower() in service["name"].lower()
        ]
        matched_by_geo_name = matched_by_name[:service_input.maxResults]  # Limit to top maxResults results
        return Hits(
            totalHits=len(matched_by_geo_name),
            totalDocuments=len(self.services),
            results=matched_by_geo_name,
        )
