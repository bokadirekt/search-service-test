import unittest

from service_processor import ServiceProcessor
from schema import Position


class TestServiceProcessor(unittest.TestCase):
    def setUp(self):
        self.sp = ServiceProcessor()

    def test_distance_calculation(self):
        user_position = Position(lat=59.3166428, lng=18.0561182999999)
        service_position = Position(lat=59.3320299, lng=18.023149800000056)
        distance = self.sp.calculate_distance(user_position, service_position)
        self.assertGreater(distance, 0)

    def test_service_score(self):
        services = [
            {
                "id": 1,
                "name": "Massage",
                "position": {"lat": 59.3166428, "lng": 18.0561182999999},
                "distance": 0.5,
            },
            {
                "id": 2,
                "name": "Salongens massage",
                "position": {"lat": 59.3320299, "lng": 18.023149800000056},
                "distance": 1.5,
            },
            {
                "id": 3,
                "name": "Massör",
                "position": {"lat": 59.315887, "lng": 18.081163800000013},
                "distance": 2.5,
            },
        ]
        service_score = self.sp.calculate_service_score(services)
        self.assertEqual(service_score[0]["score"], 5)
        self.assertEqual(service_score[1]["score"], 4)
        self.assertEqual(service_score[2]["score"], 3)


if __name__ == "__main__":
    unittest.main()
