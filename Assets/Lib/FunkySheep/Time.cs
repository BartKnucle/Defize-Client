using System;

namespace FunkySheep {
  public static class Time
  {
      
    // Get the unix epoch time (same has the javascript function)
    public static double getTime(DateTime date) {
      DateTime unixDate = new DateTime(1970, 1, 1);
      double unixTimestamp = date.ToUniversalTime().Subtract(unixDate).TotalMilliseconds;
      return Math.Round(unixTimestamp);
    }

    // Get the time unix time stamp of now
    public static double Now() {
      return getTime(DateTime.Now);
    }
  }
}
