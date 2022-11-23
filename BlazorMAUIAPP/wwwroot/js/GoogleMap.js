function initialize() {
    var latlng = new google.maps.LatLng(29.499176, 78.130051);
    var options = {
        zoom: 14, center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    var map = new google.maps.Map(document.getElementById("map"), options);
    TestMarker();
}

// Function for adding a marker to the page.
function addMarker(location) {
  var  marker = new google.maps.Marker({
        position: location,
        map: map
    });
}

// Testing the addMarker function
function TestMarker() {
    var CentralPark = new google.maps.LatLng(29.499176, 78.130051);
    addMarker(CentralPark);
}