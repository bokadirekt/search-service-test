from typing import List, Optional
from pydantic import BaseModel


class Position(BaseModel):
    lat: float
    lng: float


class ServiceInput(BaseModel):
    serviceName: str
    userLocation: Position
    maxResults: Optional[int] = 2  # Default value for maxResults is set to 2


class Results(BaseModel):
    id: int
    name: str
    position: Position
    distance: str
    score: int


class Hits(BaseModel):
    totalHits: int
    totalDocuments: int
    results: List[Results]
