# solar-system
Unity3D simulation of Earth-Sun-Moon

## Demo
![](solar-system-demo.gif)

## Play it here
https://rb.gy/lic2mb

## Features
- Tracks the positions and rotations of the Earth and the Moon for any given date and time (in UTC)
- Offers a variety of view modes for the camera to point towards:
  - **Sun view** points towards the Sun from a point on Earth's surface directly underneath the Sun
    - In this view the Sun and the Moon are accurately scaled with respect to one another to allow for solar eclipse visualization
  - **Earth view** provides a view of the Earth from a point in the sky along the vernal equinox
  - **Moon view** provides a zoomed in view of the Moon from Earth's northern hemisphere
  - **Top-down** view (default view) points down on the ecliptic plane from a point above the Sun
    - Scales of all bodies are extremely exaggerated to allow all bodies to be seen
- Playback options:
  - Play/Pause
  - Reverse
  - Speed control slider
    - Logarithmic scale to allow speed to be adjusted between x1 speed (real-time) and x31,500,000 speed (1 year/sec)
  - Date Selection Menu
    - Can set any date/time between 2000 and 2100
