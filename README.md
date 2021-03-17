# solar-system
Unity3D simulation of Earth-Sun-Moon (and hopefully more!)

## Features
- Tracks the positions and rotations of the Earth and the Moon for any given date/time
- Offers a variety of view modes for the camera to point towards:
  - Sun view points towards the Sun from Earth's vantage point
    - In this view the Sun and the Moon are accurately scaled with respect to one another which allows for solar eclipse tracking
  - Earth view points towards the Earth from a point on the sky along the vernal equinox
  - Moon view provides a zoomed in view of the Moon from Earth's surface
  - Top-down view (default view) points down on the ecliptic plane from a point above the Sun
- Playback options:
  - Play/Pause
  - Reverse
  - Speed control slider
    - Logarithmic scale to allow speed to be adjusted between 1x speed (real-time) and 30,000,000x speed (1 year/sec)
  - Date Selection Menu
    - Can set any date/time between 2000 and 2100
