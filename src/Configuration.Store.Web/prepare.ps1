Write-Host "Going to install NPM packages"

npm install

Write-Host "Going to install Yarn packages"

./node_modules/.bin/yarn install

Write-Host "Going to execute gulp default task"

./node_modules/.bin/gulp