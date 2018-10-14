import React from 'react';
import renderer from 'react-test-renderer';

import ConfigurationList from '../ConfigurationList';

test('Link changes the class when hovered', () => {
    const component = renderer.create(
        <ConfigurationList />
    );
});